using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;

namespace EMBC.ESS.Shared.Contracts.Reports
{
    public class EvacueeReportQuery : Query<ReportQueryResult>
    {
        public string ReportRequestId { get; set; }
    }

    public class SupportReportQuery : Query<ReportQueryResult>
    {
        public string ReportRequestId { get; set; }
    }

    public class PaymentReportQuery : Query<ReportQueryResponse>
    {
        public string Date { get; set; }
        public string EndDate { get; set; }
    }

    public class ReportQueryResponse
    {
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }

    public class ReportQueryResult
    {
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }

    public class RequestEvacueeReportCommand : Command
    {
        public string TaskNumber { get; set; }
        public string FileId { get; set; }
        public string EvacuatedFrom { get; set; }
        public string EvacuatedTo { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool IncludePersonalInfo { get; set; }
    }

    public class RequestSupportReportCommand : Command
    {
        public string TaskNumber { get; set; }
        public string FileId { get; set; }
        public string EvacuatedFrom { get; set; }
        public string EvacuatedTo { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class EvacueeReportRequested : Event
    {
        public string ReportRequestId { get; set; }
        public string FileId { get; set; }
        public string TaskNumber { get; set; }
        public string EvacuatedFrom { get; set; }
        public string EvacuatedTo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IncludePersonalInfo { get; set; }
    }

    public class SupportReportRequested : Event
    {
        public string ReportRequestId { get; set; }
        public string FileId { get; set; }
        public string TaskNumber { get; set; }
        public string EvacuatedFrom { get; set; }
        public string EvacuatedTo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public object IncludePersonalInfo { get; set; }
    }
}
