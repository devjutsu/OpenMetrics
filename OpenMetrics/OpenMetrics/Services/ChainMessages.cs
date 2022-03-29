using MetaMask.Blazor;
using MetaMask.Blazor.Enums;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;

namespace OpenMetrics.Services;

[Function("metricsCount", "uint256")]
public class MetricsCount : FunctionMessage
{
}

//[Function("checkMints", "uint256")]
//public class CheckMintsFunction : FunctionMessage
//{
//    [Parameter("address", "_player", 1)] public string Player { get; set; }
//}

//[Function("checkProfit", "uint256")]
//public class CheckProfitFunction : FunctionMessage
//{
//    [Parameter("bytes6", "_invite", 1)] public byte[] Invite { get; set; }
//}

//[Function("verify", "bool")]
//public class VerifyFunction : FunctionMessage
//{
//    [Parameter("address", "_signer", 1)] public string Signer { get; set; }
//    [Parameter("bytes6", "_invite", 1)] public byte[] Invite { get; set; }
//    [Parameter("bytes", "_sig", 1)] public byte[] Sig { get; set; }
//}