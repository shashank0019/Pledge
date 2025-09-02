using System;
namespace Pledge.MODELS
{
    // DTO for fetching pledge details
    public class SecurityPledgeDetailsDto
    {
        public int InstanceId { get; set; }
        public string? SPName { get; set; }
        public string? SPCheckText { get; set; }
        public string? SPHtmlBody { get; set; }
        public string? WFStatus { get; set; }
        public bool? IsAgree { get; set; }
        public string? EmpName { get; set; }
        public bool IsUploadAttachmentAvailable { get; set; }
        public int? FileIndex { get; set; }
        public bool IsTimerEnabled { get; set; }
        public int? TimeInSecs { get; set; }
        public int SPID { get; set; }
    }

    // DTO for submitting pledge response
    public class SecurityPledgeSubmitDto
    {
        public int InstanceId { get; set; }
        public bool IsAgree { get; set; }
    }
}

