﻿using OpenMetrics.ViewModels;

namespace OpenMetrics.Services
{
    public static class MappingExtensions
    {
        public static Metric DtoToMetric(this MetricsDTO metricDto, ulong id)
        {
            return new Metric()
            {
                Checksum = metricDto.Checksum,
                Cid = metricDto.Cid,
                Status = metricDto.Status,
                Id = id
            };
        }

        public static string StatusText(this HistoryRecordDTO dto)
        {
            switch((ulong)dto.Status)
            {
                case 1:
                    return "Posted";
                case 2:
                    return "Approved";
                case 3:
                    return "Rejected";
                default:
                    return "None";
            }
        }
    }
}
