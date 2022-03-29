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

namespace OpenMetrics.Services
{
    public interface IChain
    {
        Task<BigInteger> MetricsCount();
        Task<string> SubmitMetric();
    }

    public class ChainClient : IChain
    {
        private readonly AppState _state;
        private readonly IToastService _toast;
        private readonly ClientConfig _config;
        private readonly IJSRuntime _js;
        private readonly MetaMaskService _meta;

        public ChainClient(AppState state, IToastService toast, ClientConfig config, IJSRuntime js, MetaMaskService meta)
        {
            _state = state;
            _toast = toast;
            _config = config;
            _js = js;
            _meta = meta;
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

        public async Task<string> SubmitMetric(string cid)
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



                var values = new object[] { cid, hash.HexToByteArray() };

                var receipt = await _meta.SendTransactionAndWaitForReceipt(web3.Client,
                                                            ContractFunctionNames.SubmitMetricFunction,
                                                            _config.ContractAddress,
                                                            Web3.Convert.ToWei(0),
                                                            parameters,
                                                            values);

                return receipt.TransactionHash.ToString();

                //var queryHandler = web3.Eth.GetContractQueryHandler<MetricsCount>();
                //var resp = await queryHandler
                //                    .QueryAsync<BigInteger>(_config.ContractAddress, args)
                //                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
            return null;
        }
    }
}
