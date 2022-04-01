using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OpenMetrics.ViewModels;

namespace OpenMetrics.Services
{
    public class AppState
    {
        public long ChainId { get; private set; } = 1;
        public string AccountId {get; private set;}
        public ulong MetricsCount { get; private set; } = 0;
        public ulong ApprovedCount { get; private set; } = 0;
        public List<ulong> ApprovedIds { get; private set; } = new List<ulong>();
        public ulong UncheckedCount { get; private set; } = 0;
        public AppScreenMode ScreenMode { get; private set; } = AppScreenMode.All;
        public Metric SelectedMetric { get; private set; } = null;
        public List<Metric> Metrics { get; private set; } = new List<Metric>();

        public List<Metric> ApprovedMetrics => Metrics.Where(o => ApprovedIds.Contains(o.Id)).ToList();
        public List<Metric> UncheckedMetrics => Metrics.Where(o => !ApprovedIds.Contains(o.Id)).ToList();

        public bool IsAuthenticated => AccountId != null && !string.IsNullOrWhiteSpace(AccountId);
        public bool IsSuperuser => AccountId.ToLower() == "0x47B40160f72C4321E08DE8B95E262e902c991cD3".ToLower();

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

        public void SetApprovedCount(ComponentBase source, ulong approvedCount)
        {
            this.ApprovedCount = approvedCount;
            this.UncheckedCount =  MetricsCount > 0 ? (MetricsCount - approvedCount) : 0;
            NotifyStateChanged(source, "ApprovedCount");
            NotifyStateChanged(source, "UncheckedCount");
        }

        public void AddMetric(ComponentBase source, Metric metric)
        {
            this.Metrics.Add(metric);
            NotifyStateChanged(source, "Metrics");
        }

        public void SetMetrics(ComponentBase source, List<Metric> metrics)
        {
            this.Metrics = metrics;
            NotifyStateChanged(source, "Metrics");
        }

        public void SetSelectedMetric(ComponentBase source, Metric metric)
        {
            this.SelectedMetric = metric;
            NotifyStateChanged(source, "SelectedMetric");
        }

        public void SetScreenMode(ComponentBase source, AppScreenMode mode)
        {
            this.ScreenMode = mode;
            SelectedMetric = null;
            NotifyStateChanged(source, "ScreenMode");
            NotifyStateChanged(source, "SelectedMetric");
        }

        public void SetChain(ComponentBase source, long chainId)
        {
            this.ChainId = chainId;
            NotifyStateChanged(source, "ChainId");
        }

        public void SetApprovedMetricIds(ComponentBase source, List<ulong> approvedIds)
        {
            this.ApprovedIds = approvedIds;
            NotifyStateChanged(source, "ApprovedMetricIds");
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
        Verified = 0,
        All = 1,
        Unchecked = 2,
    }
}
