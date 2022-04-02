using MetaMask.Blazor;
using MetaMask.Blazor.Enums;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;
using System.Numerics;

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

[Function("getHistoryRecordCount", "uint256")]
public class GetHistoryRecordCount : FunctionMessage
{
    [Parameter("uint256", "_id", 1)]
    public BigInteger Id { get; set; }
}