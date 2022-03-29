using Nethereum.Web3;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3.Accounts;
using Nethereum.Contracts;
using Blazored.Toast.Services;

namespace OpenMetrics.Services
{
    public interface IChain
    {
        //Task<bool> GetData();
    }

    public class ChainClient : IChain
    {
        private readonly AppState _state;
        private readonly IToastService _toast;
        private readonly ClientConfig _config;

        public ChainClient(AppState state, IToastService toast, ClientConfig config)
        {
            _state = state;
            _toast = toast;
            _config = config;
        }

        
    }
}
