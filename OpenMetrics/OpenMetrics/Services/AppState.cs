using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace OpenMetrics.Services
{
    public class AppState
    {
        public long ChainId { get; private set; } = 1;
        public string AccountId {get; private set;}
        public bool IsAuthenticated => AccountId != null && !string.IsNullOrWhiteSpace(AccountId);

        public string AccountToShow()
        {
            if ((string.IsNullOrEmpty(AccountId)))
                return string.Empty;
            else
                return $"{AccountId.Substring(0, 6)}...{AccountId.Substring(AccountId.Length - 4)}";
        }

        private IJSRuntime _js;

        public AppState(IJSRuntime JS)
        {
            _js = JS;
        }

        public void SetChain(ComponentBase source, long chainId)
        {
            this.ChainId = chainId;
            NotifyStateChanged(source, "ChainId");
        }

        public async Task Login(ComponentBase source, string address)
        {
            AccountId = address;
            NotifyStateChanged(source, "CurrentUser");
        }

        public event Action<ComponentBase, string> Statechanged;
        private void NotifyStateChanged(ComponentBase source, string Property) =>
            Statechanged?.Invoke(source, Property);
    }
}
