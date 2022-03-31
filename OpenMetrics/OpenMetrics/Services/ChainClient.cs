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
        Task GetTransactions(ulong id);
        Task<BigInteger> ApprovedCount();
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
                    _toast.ShowError($"Please, switch to chain: {NetworksList.Networks[_config.NetworkId]}");
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
                var web3 = new Web3(_config.RpcUrl);
                var args = new ApprovesCount() { };
                var queryHandler = web3.Eth.GetContractQueryHandler<ApprovesCount>();
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

        public async Task GetTransactions(ulong id)
        {
            _toast.ShowInfo("Trying to get history");
            var web3 = new Web3(_config.RpcUrl);
            var transactionEventHandler = web3.Eth.GetEvent<TransactionEventDTO>(_config.ContractAddress);
            //var filter = transactionEventHandler.CreateFilterInput(
            //    fromBlock: new BlockParameter(25746500)
            //    //toBlock: new BlockParameter(25746540)
            //    );


            var filterInput = transactionEventHandler.CreateFilterInput();
            var topic = filterInput.Topics.FirstOrDefault();
            Console.WriteLine($"Got: {topic}");
            Console.WriteLine($"addr: {filterInput.Address[0]}");

            //var allEvents = await transactionEventHandler.GetAllChangesAsync(filterInput);
            //Console.WriteLine($"allEvents: {allEvents.Count}");


            //var filter = transactionEventHandler.CreateFilterInput();
            //var logs = await transactionEventHandler.GetAllChangesAsync(filter);
            //Console.WriteLine($"Got events: {logs.Count}");


            //var filter = transactionEventHandler.CreateFilterInput(fromBlock: new BlockParameter(25746530));
            //var logs = await transactionEventHandler.GetAllChangesAsync(filter);
            //Console.WriteLine($"Got events: {logs.Count}");

            //foreach (var logItem in logs)
            //    Console.WriteLine(
            //        $"tx:{logItem.Log.TransactionHash} " +
            //        $"id:{logItem.Event.Id} " +
            //        $"cid:{logItem.Event.Cid} " //+
            //        //$"type:{logItem.Event.ChangeType}"
            //        );

            // var transferEventHandler = web3.Eth.GetEvent<TransferEventDTO>(contractAddress);
            // var filterAllTransferEventsForContract = transferEventHandler.CreateFilterInput();
            //  var allTransferEventsForContract = await transferEventHandler.GetAllChanges(filterAllTransferEventsForContract);
        }
    }
}
