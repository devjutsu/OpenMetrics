using MetaMask.Blazor;
using MetaMask.Blazor.Enums;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;

namespace OpenMetrics.Services
{
    [FunctionOutput]
    public class MetricsDTO : IFunctionOutputDTO
    {
        [Parameter("address", "creator", 1)]
        public string Creator { get; set; }

        [Parameter("address", "editor", 2)]
        public string Editor { get; set; }

        [Parameter("address", "approver", 3)]
        public string Approver { get; set; }

        [Parameter("uint8", "status", 4)]
        public int Status { get; set; }

        [Parameter("string", "cid", 5)]
        public string Cid { get; set; }

        [Parameter("bytes32", "checksum", 6)]
        public byte[] Checksum { get; set; }

        //[Parameter("uint256", "id", 1)]
        //public int Id { get; set; }
    }


    public class TransactionEventDTO
    {
        public ulong Id { get; set; }
        public string Author { get; set; }
        public int ChangeType { get; set; }
        public string Cid { get; set; }
    }
}
