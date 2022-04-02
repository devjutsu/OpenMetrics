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
        [Parameter("uint8", "status", 1)]
        public int Status { get; set; }

        [Parameter("string", "cid", 2)]
        public string Cid { get; set; }

        [Parameter("bytes32", "checksum", 3)]
        public byte[] Checksum { get; set; }

        //[Parameter("uint256", "id", 1)]
        //public int Id { get; set; }
    }

    [FunctionOutput]
    public class ApprovedListDTO : IFunctionOutputDTO
    {
        [Parameter("uint256[]", "approved", 1)]
        public List<BigInteger> Approved { get; set; }
    }

    [FunctionOutput]
    public class HistoryDTO : IFunctionOutputDTO
    {
        [Parameter("tuple[]", "history", 1)]
        public List<HistoryRecordDTO> Records { get; set; }
    }

    [FunctionOutput]
    public class HistoryRecordDTO : IFunctionOutputDTO
    {
        [Parameter("uint256", "id", 1)]
        public BigInteger Id { get; set; }
        [Parameter("address", "author", 2)]
        public string Author { get; set; }

        [Parameter("uint8", "status", 3)]
        public BigInteger Status { get; set; }
        [Parameter("uint256", "timestamp", 4)]
        public BigInteger Timestamp { get; set; }
        //[Parameter("string", "cid", 4)]
        //public string Cid { get; set; }
    }
}
