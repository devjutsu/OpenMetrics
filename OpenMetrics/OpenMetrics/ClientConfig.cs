namespace OpenMetrics
{
    public class ClientConfig
    {
        public string ApiUrl { get; set; }
        public long NetworkId { get; set; }

        //public string ContractAddress { get; set; }
        //public string RpcUrl { get; set; }
        //public string ChainId { get; set; }
    }

    public static class NetworksList
    {
        public static Dictionary<long, string> Networks = new Dictionary<long, string>
        {
            { 0x1, "Mainnet" },
            { 0x3, "Ropsten" },
            { 0x4, "Rinkeby" },
            { 0x61, "BSC Testnet"},
            { 0x38, "BSC Mainnet"},
            { 0x89, "Polygon Mainnet"},
            { 0x13881, "Polygon Mumbai Testnet"}
        };
    }
}
