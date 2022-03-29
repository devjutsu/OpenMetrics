namespace OpenMetrics.ViewModels
{
    public class Metric
    {
        public int Id { get; set; }
        public string Cid { get; set; }
        public string Creator { get; set; }
        public string Editor { get; set; }
        public string Approver { get; set; }
        public int Status { get; set; }
        public byte[] Checksum { get; set; }
    }
}
