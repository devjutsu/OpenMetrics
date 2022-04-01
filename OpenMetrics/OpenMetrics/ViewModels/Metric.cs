namespace OpenMetrics.ViewModels
{
    public class Metric
    {
        public ulong Id { get; set; }
        public string Cid { get; set; }
        public int Status { get; set; }
        public byte[] Checksum { get; set; }
    }
}
