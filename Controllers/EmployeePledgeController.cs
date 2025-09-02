using Microsoft.AspNetCore.Mvc;
using Pledge.BUSINESSLOGIC;
using Pledge.MODELS;

namespace Pledge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeePledgeController : ControllerBase
    {
        private readonly EmployeePledgeBusinessLogic _businessLogic;

        public EmployeePledgeController(EmployeePledgeBusinessLogic businessLogic)
        {
            _businessLogic = businessLogic;
        }

        // GET: api/EmployeePledge/details?cid=3283
        [HttpGet("details")]
        public async Task<ActionResult<SecurityPledgeDetailsDto>> GetDetails([FromQuery] int cid)
        {
            var details = await _businessLogic.GetPledgeDetailsAsync(cid);
            if (details == null)
                return NotFound(new { message = "Pledge details not found" });

            return Ok(details);
        }

        // POST: api/EmployeePledge/submit
        [HttpPost("submit")]
        public async Task<ActionResult> SubmitPledge([FromBody] SecurityPledgeSubmitDto pledge)
        {
            var result = await _businessLogic.SubmitPledgeAsync(pledge);
            if (!result)
                return BadRequest(new { message = "Failed to submit pledge" });

            return Ok(new { message = "Pledge submitted successfully" });
        }
    }
}
