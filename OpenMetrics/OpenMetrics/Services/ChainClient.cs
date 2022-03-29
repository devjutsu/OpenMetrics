using Nethereum.Web3;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3.Accounts;
using Nethereum.Contracts;
using Blazored.Toast.Services;
using Microsoft.JSInterop;
using System.Numerics;
using OpenMetrics.ViewModels;

namespace OpenMetrics.Services
{
    public interface IChain
    {
        Task<BigInteger> MetricsCount();
    }

    public class ChainClient : IChain
    {
        private readonly AppState _state;
        private readonly IToastService _toast;
        private readonly ClientConfig _config;
        private readonly IJSRuntime _js;

        public ChainClient(AppState state, IToastService toast, ClientConfig config, IJSRuntime js)
        {
            _state = state;
            _toast = toast;
            _config = config;
            _js = js;
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
    }
}
