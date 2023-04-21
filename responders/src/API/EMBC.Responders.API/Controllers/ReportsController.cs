using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EMBC.ESS.Shared.Contracts.Events;
using EMBC.ESS.Shared.Contracts.Reports;
using EMBC.ESS.Shared.Contracts.Teams;
using EMBC.Responders.API.Helpers;
using EMBC.Utilities.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMBC.Responders.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IMessagingClient messagingClient;

        private string currentUserRole => User.FindFirstValue("user_role");
        private readonly ErrorParser errorParser;
        private readonly IMapper mapper;

        public ReportsController(IMessagingClient messagingClient, IMapper mapper)
        {
            this.messagingClient = messagingClient;
            this.errorParser = new ErrorParser();
            this.mapper = mapper;
        }

        [HttpPost("evacuee")]
        public async Task<string?> CreateEvacueeReport(string? taskNumber, string? fileId, string? evacuatedFrom, string? evacuatedTo, DateTime? from, DateTime? to)
        {
            var userRole = Enum.Parse<MemberRole>(currentUserRole);
            var includePersonalInfo = userRole == MemberRole.Tier3 || userRole == MemberRole.Tier4;

            var requestId = await messagingClient.Send(new RequestEvacueeReportCommand
            {
                TaskNumber = taskNumber,
                FileId = fileId,
                EvacuatedFrom = evacuatedFrom,
                EvacuatedTo = evacuatedTo,
                From = from,
                To = to,
                IncludePersonalInfo = includePersonalInfo
            });

            return requestId;
        }

        [HttpGet("evacuee")]
        public async Task<IActionResult> GetEvacueeReport(string reportRequestId)
        {
            try
            {
                var report = await messagingClient.Send(new EvacueeReportQuery { ReportRequestId = reportRequestId });

                return new FileContentResult(report.Content, report.ContentType);
            }
            catch (Exception e)
            {
                return errorParser.Parse(e);
            }
        }

        [HttpPost("support")]
        public async Task<string?> CreateSupportReport(string? taskNumber, string? fileId, string? evacuatedFrom, string? evacuatedTo, DateTime? from, DateTime? to)
        {
            var requestId = await messagingClient.Send(new RequestSupportReportCommand
            {
                TaskNumber = taskNumber,
                FileId = fileId,
                EvacuatedFrom = evacuatedFrom,
                EvacuatedTo = evacuatedTo,
                From = from,
                To = to
            });

            return requestId;
        }

        [HttpGet("support")]
        public async Task<IActionResult> GetSupportReport(string reportRequestId)
        {
            try
            {
                var report = await messagingClient.Send(new SupportReportQuery { ReportRequestId = reportRequestId });

                return new FileContentResult(report.Content, report.ContentType);
            }
            catch (Exception e)
            {
                return errorParser.Parse(e);
            }
        }

        /// <summary>
        /// Get 3cmb report
        /// </summary>
        /// <returns>list of epayment transactions</returns>
        [HttpGet("3cmb")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Get3cmbReport()
        {
            //var response = await client.Send(new TeamMembersQuery { TeamId = teamId, IncludeActiveUsersOnly = false });
            var response = await messagingClient.Send(new PaymentReportQuery { Date = "01/01/2023" });
            return new FileContentResult(response.Content, response.ContentType);
            //return Ok(mapper.Map<TeamMember>(response));
        }
    }
}
