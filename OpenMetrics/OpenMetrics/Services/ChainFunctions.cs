using MetaMask.Blazor;
using MetaMask.Blazor.Enums;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;

namespace OpenMetrics.Services;

public static class ContractFunctionNames
{
    public static string SubmitMetricFunction { get; } = "submitMetric";
    public static string ApproveMetricFunction { get; } = "approveMetric";
}

[Function("metricsCount", "uint256")]
public class MetricsCount : FunctionMessage
{
}

[Function("countApproved", "uint256")]
public class ApprovedCount : FunctionMessage
{
}


//function approveMetric(uint256 _id) public {
//[Function("approveMetric", "uint256")]
//public class ApproveMetricFunction : FunctionMessage
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