﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace OpenMetrics.Services
{
    public class AppState
    {
        public long ChainId { get; private set; } = 1;
        public string AccountId {get; private set;}
        public bool IsAuthenticated => AccountId != null && !string.IsNullOrWhiteSpace(AccountId);
        public ulong MetricsCount { get; private set; } = 0;
        public bool IsSuperuser => AccountId.ToLower() == "0x47B40160f72C4321E08DE8B95E262e902c991cD3".ToLower();
        public AppScreenMode ScreenMode { get; private set; } = AppScreenMode.DefaultVerified;


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

        public void SetMetricCount(ComponentBase source, ulong metricsCount)
        {
            this.MetricsCount = metricsCount;
            NotifyStateChanged(source, "MetricCount");
        }

        public void SetScreenMode(ComponentBase source, AppScreenMode mode)
        {
            this.ScreenMode = mode;
            NotifyStateChanged(source, "ScreenMode");
        }

        public void SetChain(ComponentBase source, long chainId)
        {
            this.ChainId = chainId;
            NotifyStateChanged(source, "ChainId");
        }

        public async Task Login(ComponentBase source, string address)
        {
            AccountId = address.ToLower();
            NotifyStateChanged(source, "CurrentUser");
        }

        public event Action<ComponentBase, string> Statechanged;
        private void NotifyStateChanged(ComponentBase source, string Property) =>
            Statechanged?.Invoke(source, Property);
    }

    public enum AppScreenMode
    {
        DefaultVerified = 0,
        All = 1,
        Unchecked = 2,
    }
}
