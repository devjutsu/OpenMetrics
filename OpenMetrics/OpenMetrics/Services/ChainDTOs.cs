using MetaMask.Blazor;
using MetaMask.Blazor.Enums;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;
using System.Numerics;

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

    [Event("Transaction")]
    public class TransactionEventDTO : IEventDTO
    {
        [Parameter("uint256", "id", 1, false)]
        public BigInteger Id { get; set; }
        //[Parameter("address", "_from", 2, true)]
        //public string author { get; set; }
        //[Parameter("uint8", "changeType", 3, false)]
        //public int ChangeType { get; set; }
        [Parameter("string", "cid", 2, true)]
        public string Cid { get; set; }
    }
}
