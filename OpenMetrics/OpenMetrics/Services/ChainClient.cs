using Nethereum.Web3;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3.Accounts;
using Nethereum.Contracts;
using Blazored.Toast.Services;
using Microsoft.JSInterop;
using System.Numerics;
using OpenMetrics.ViewModels;
using Nethereum.ABI.Model;
using Nethereum.Hex.HexConvertors.Extensions;
using MetaMask.Blazor;
using Nethereum.RPC.Eth.DTOs;

namespace OpenMetrics.Services
{
    public interface IChain
    {
        Task<BigInteger> MetricsCount();
        Task<string> SubmitMetric(Metric metric);
        Task<Metric> GetMetric(ulong id);
        Task<bool> ApproveMetric(ulong id);
        Task<BigInteger> ApprovedCount();
        Task<List<ulong>> GetApprovedMetrics();
        Task<ulong> GetHistoryRecordsCount(ulong id);
        Task<List<HistoryRecordDTO>> GetHistory(ulong id);
        Task<HistoryRecordDTO> GetHistoryRecord(ulong id, ulong n);
    }

    public class ChainClient : IChain
    {
        private readonly AppState _state;
        private readonly IToastService _toast;
        private readonly ClientConfig _config;
        private readonly IJSRuntime _js;
        private readonly MetaMaskService _meta;
        private readonly HttpClient _http;

        public ChainClient(AppState state, IToastService toast, ClientConfig config, IJSRuntime js, MetaMaskService meta, HttpClient http)
        {
            _state = state;
            _toast = toast;
            _config = config;
            _js = js;
            _meta = meta;
            _http = http;
        }

        public async Task<BigInteger> MetricsCount()
        {
            try
            {
                if (_state.ChainId != _config.NetworkId)
                {
                    return 0;
                }

                var web3 = new Web3(_config.RpcUrl);
                var args = new MetricsCount() { };
                var queryHandler = web3.Eth.GetContractQueryHandler<MetricsCount>();
                var resp = await queryHandler
                                    .QueryAsync<BigInteger>(_config.ContractAddress, args)
                                    .ConfigureAwait(false);

                Console.WriteLine($"Metrics count: {resp}");
                return resp;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
            return 0;
        }

        public async Task<Metric> GetMetric(ulong id)
        {
            var web3 = new Web3(_config.RpcUrl);

            var abi = await _http.GetStringAsync("abi.json");
            var contract = web3.Eth.GetContract(abi, _config.ContractAddress);

            var tmp = contract.GetFunction("Metrics");
            var tst = await tmp.CallDeserializingToObjectAsync<MetricsDTO>(id);

            var metric = tst.DtoToMetric(id);
            return metric;
        }

        public async Task<BigInteger> ApprovedCount()
        {
            try
            {
                Console.WriteLine("Checking approved");
                var web3 = new Web3(_config.RpcUrl);
                var args = new ApprovedCount() { };
                var queryHandler = web3.Eth.GetContractQueryHandler<ApprovedCount>();
                var resp = await queryHandler
                                    .QueryAsync<BigInteger>(_config.ContractAddress, args)
                                    .ConfigureAwait(false);

                Console.WriteLine($"Approved count: {resp}");
                return resp;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
            return 0;
        }

        public async Task<List<ulong>> GetApprovedMetrics()
        {
            var web3 = new Web3(_config.RpcUrl);

            var abi = await _http.GetStringAsync("abi.json");
            var contract = web3.Eth.GetContract(abi, _config.ContractAddress);

            Console.WriteLine($"Debug 0");
            var funcHandler = contract.GetFunction("metricsApproved");
            Console.WriteLine($"Debug 1");
            var result = await funcHandler.CallDeserializingToObjectAsync<ApprovedListDTO>();
            Console.WriteLine($"Debug 2");

            if (result.Approved == null)
            {
                Console.WriteLine("nil");
                return new List<ulong>();
            }

            var ids = result.Approved.Select(o => (ulong)o).ToList();

            Console.WriteLine($"got approved: {result.Approved.Count}");
            return ids;
        }

        public async Task<string> SubmitMetric(Metric metric)
        {
            var hash = "0x1234567812345678123456781234567812345678123456781234567812345678";

            try
            {
                if (_state.ChainId != _config.NetworkId)
                {
                    _toast.ShowError($"Please, switch to chain: {NetworksList.Networks[_config.NetworkId]}");
                    return null;
                }

                var web3 = new Web3(_config.RpcUrl);
                var parameters = new Parameter[] {
                    new Parameter(type: "string", name: "_cid", order: 1),
                    new Parameter(type: "bytes32", name: "_checksum", order: 2)
                };

                Console.WriteLine("CALL SUBMIT");

                var values = new object[] { metric.Cid, hash.HexToByteArray() };

                var receipt = await _meta.SendTransactionAndWaitForReceipt(web3.Client,
                                                            ContractFunctionNames.SubmitMetricFunction,
                                                            _config.ContractAddress,
                                                            Web3.Convert.ToWei(0),
                                                            parameters,
                                                            values);

                return receipt.TransactionHash.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
            return null;
        }

        public async Task<bool> ApproveMetric(ulong id)
        {
            try
            {
                if (_state.ChainId != _config.NetworkId)
                {
                    _toast.ShowError($"Please, switch to chain: {NetworksList.Networks[_config.NetworkId]}");
                    return false;
                }

                Console.WriteLine($"Call approve for id: {id}");

                var web3 = new Web3(_config.RpcUrl);
                var parameters = new Parameter[] {
                    new Parameter(type: "uint256", name: "_cid", order: 1),
                };

                Console.WriteLine("CALL APPROVE");

                var values = new object[] { id };

                var receipt = await _meta.SendTransactionAndWaitForReceipt(web3.Client,
                                                            ContractFunctionNames.ApproveMetricFunction,
                                                            _config.ContractAddress,
                                                            Web3.Convert.ToWei(0),
                                                            parameters,
                                                            values);
                if (receipt.Status == BigInteger.One)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
            return false;
        }

        public async Task<ulong> GetHistoryRecordsCount(ulong id)
        {
            _toast.ShowInfo("Trying to get history");
            var web3 = new Web3(_config.RpcUrl);
            var queryHandler = web3.Eth.GetContractQueryHandler<GetHistoryRecordCount>();
            var args = new GetHistoryRecordCount() { Id = new BigInteger(id) };
            var resp = await queryHandler
                                .QueryAsync<BigInteger>(_config.ContractAddress, args)
                                .ConfigureAwait(false);

            Console.WriteLine($"History Records Count: {resp}");
            return (uint)resp;
        }

        public async Task<List<HistoryRecordDTO>> GetHistory(ulong id)
        {
            var web3 = new Web3(_config.RpcUrl);

            var abi = await _http.GetStringAsync("abi.json");
            var contract = web3.Eth.GetContract(abi, _config.ContractAddress);

            Console.WriteLine($"Debug GetHistory for id {id}");
            var funcHandler = contract.GetFunction("getHistory");
            Console.WriteLine($"Debug 1");
            var result = await funcHandler.CallDeserializingToObjectAsync<HistoryDTO>(id);
            Console.WriteLine($"Debug 2");

            if (result?.Records == null)
            {
                Console.WriteLine("nil");
                return new List<HistoryRecordDTO>();
            }
            else
            {
                return result.Records;
            }
        }

        public async Task<HistoryRecordDTO> GetHistoryRecord(ulong id, ulong n)
        {
            var web3 = new Web3(_config.RpcUrl);

            var abi = await _http.GetStringAsync("abi.json");
            var contract = web3.Eth.GetContract(abi, _config.ContractAddress);

            Console.WriteLine($"Debug GetHistoryRecord for id {id}");
            var funcHandler = contract.GetFunction("getHistoryRecord");
            Console.WriteLine($"Debug 1");
            var parameters = new Parameter[] { 
                new Parameter(type: "uint256", name: "_id", order: 1),
                new Parameter(type: "uint256", name: "_n", order: 2)
            };

            var result = await funcHandler.CallDeserializingToObjectAsync<HistoryRecordDTO>(parameters);
            Console.WriteLine($"Debug 2");

            return result;
        }

    }
}
