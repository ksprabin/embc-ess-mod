using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Execution;
using EMBC.ESS.Resources.Metadata;
using EMBC.ESS.Resources.Payments;
using EMBC.ESS.Resources.Reports;
using EMBC.ESS.Resources.Teams;
using EMBC.ESS.Shared.Contracts;
using EMBC.ESS.Shared.Contracts.Reports;
using EMBC.ESS.Shared.Contracts.Teams;
using EMBC.Utilities.Caching;
using EMBC.Utilities.Csv;
using EMBC.Utilities.Messaging;

namespace EMBC.ESS.Managers.Reports
{
    public class ReportsManager
    {
        private readonly IMapper mapper;
        private readonly IPaymentRepository paymentRepository;
        private readonly IReportRepository reportRepository;
        private readonly IMetadataRepository metadataRepository;
        private readonly ITeamRepository teamRepository;
        private readonly ICache cache;
        private readonly IMessagingClient messagingClient;

        public ReportsManager(
            IMapper mapper,
            IPaymentRepository paymentRepository,
            ITeamRepository teamRepository,
            IReportRepository reportRepository,
            IMetadataRepository metadataRepository,
            ICache cache,
            IMessagingClient messagingClient)
        {
            this.mapper = mapper;
            this.paymentRepository = paymentRepository;
            this.teamRepository = teamRepository;
            this.reportRepository = reportRepository;
            this.metadataRepository = metadataRepository;
            this.cache = cache;
            this.messagingClient = messagingClient;
        }

        private string ReportRequestKey(string requestId) => $"report_{requestId}";

        public async Task<string> Handle(RequestEvacueeReportCommand cmd)
        {
            var requestId = Guid.NewGuid().ToString();

            await messagingClient.Publish(new EvacueeReportRequested
            {
                ReportRequestId = requestId,
                FileId = cmd.FileId,
                TaskNumber = cmd.TaskNumber,
                EvacuatedFrom = cmd.EvacuatedFrom,
                EvacuatedTo = cmd.EvacuatedTo,
                StartDate = cmd.From,
                EndDate = cmd.To,
                IncludePersonalInfo = cmd.IncludePersonalInfo
            });

            return requestId;
        }

        public async Task Handle(EvacueeReportRequested evt)
        {
            var evacueeQuery = new ReportQuery
            {
                FileId = evt.FileId,
                TaskNumber = evt.TaskNumber,
                EvacuatedFrom = evt.EvacuatedFrom,
                EvacuatedTo = evt.EvacuatedTo,
                StartDate = evt.StartDate,
                EndDate = evt.EndDate,
            };

            var results = (await reportRepository.QueryEvacuee(evacueeQuery)).Items;
            var evacuees = mapper.Map<IEnumerable<Evacuee>>(results, opt => opt.Items["IncludePersonalInfo"] = evt.IncludePersonalInfo.ToString());

            var communities = await metadataRepository.GetCommunities();
            evacueeQuery.EvacuatedFrom = communities.SingleOrDefault(c => c.Code == evacueeQuery.EvacuatedFrom)?.Name;
            evacueeQuery.EvacuatedTo = communities.SingleOrDefault(c => c.Code == evacueeQuery.EvacuatedTo)?.Name;

            var csv = evacuees.ToCSV(evacueeQuery);

            var content = Encoding.UTF8.GetBytes(csv);
            var contentType = "text/csv";

            var report = new ReportQueryResult
            {
                Content = content,
                ContentType = contentType
            };
            var cacheKey = ReportRequestKey(evt.ReportRequestId);
            await cache.Set(cacheKey, report, TimeSpan.FromHours(1));
        }

        public async Task<ReportQueryResult> Handle(EvacueeReportQuery query)
        {
            var cacheKey = ReportRequestKey(query.ReportRequestId);
            var report = await cache.Get<ReportQueryResult>(cacheKey);

            if (report == null) throw new NotFoundException("Report is not ready", query.ReportRequestId);

            return report;
        }

        public async Task<string> Handle(RequestSupportReportCommand cmd)
        {
            var requestId = Guid.NewGuid().ToString();

            await messagingClient.Publish(new SupportReportRequested
            {
                ReportRequestId = requestId,
                FileId = cmd.FileId,
                TaskNumber = cmd.TaskNumber,
                EvacuatedFrom = cmd.EvacuatedFrom,
                EvacuatedTo = cmd.EvacuatedTo,
                StartDate = cmd.From,
                EndDate = cmd.To
            });

            return requestId;
        }

        public async Task Handle(SupportReportRequested evt)
        {
            var supportQuery = new ReportQuery
            {
                FileId = evt.FileId,
                TaskNumber = evt.TaskNumber,
                EvacuatedFrom = evt.EvacuatedFrom,
                EvacuatedTo = evt.EvacuatedTo,
                StartDate = evt.StartDate,
                EndDate = evt.EndDate,
            };

            var supports = (await reportRepository.QuerySupport(supportQuery)).Items;

            var communities = await metadataRepository.GetCommunities();
            supportQuery.EvacuatedFrom = communities.SingleOrDefault(c => c.Code == supportQuery.EvacuatedFrom)?.Name;
            supportQuery.EvacuatedTo = communities.SingleOrDefault(c => c.Code == supportQuery.EvacuatedTo)?.Name;

            var csv = supports.ToCSV(supportQuery, "\"");

            var content = Encoding.UTF8.GetBytes(csv);
            var contentType = "text/csv";

            var report = new ReportQueryResult
            {
                Content = content,
                ContentType = contentType
            };
            var cacheKey = ReportRequestKey(evt.ReportRequestId);
            await cache.Set(cacheKey, report, TimeSpan.FromHours(1));
        }

        public async Task<ReportQueryResult> Handle(SupportReportQuery query)
        {
            var cacheKey = ReportRequestKey(query.ReportRequestId);
            var report = await cache.Get<ReportQueryResult>(cacheKey);

            if (report == null) throw new NotFoundException("Report is not ready", query.ReportRequestId);

            return report;
        }

        public async Task<ReportQueryResponse> Handle(PaymentReportQuery evt)
        {
            var paymentQuery = new ReportQuery
            {
                //FileId = evt.TeamId,
                //TaskNumber = "test123",
            };

            var lstPayments = ((SearchAllPaymentResponse)await paymentRepository.QueryAll(new SearchAllPaymentRequest
            {
                ByStatus = PaymentStatus.Paid
            })).Items;

            var csv = lstPayments.ToCSV(paymentQuery);

            var content = Encoding.UTF8.GetBytes(csv);
            var contentType = "text/csv";

            var testResVal = new ReportQueryResponse
            {
                Content = content,
                ContentType = contentType
            };

            return testResVal;
        }
    }
}
