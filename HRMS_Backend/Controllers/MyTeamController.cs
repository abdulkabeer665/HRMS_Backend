using HRMS_Backend.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static HRMS_Backend.BAL.RequestParameters;
using static HRMS_Backend.BAL.ResponseParameters;

namespace HRMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyTeamController : ControllerBase
    {

        #region "Declaration"

        public readonly static string SP_GetMyTeamInfo = "[dbo].[SP_GetMyTeamInfo]";

        #endregion

        #region Get My Team Info

        /// <summary>
        /// Get Leaves availed against leave type and emp id
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost("GetMyTeamInfo")]
        [Authorize]
        public async Task<IActionResult> GetMyTeamInfo([FromBody] ReportingManagerReqParams reportingMgrReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = await DataLogic.GetMyTeamInfo(reportingMgrReqParams, SP_GetMyTeamInfo);
                return Ok(dt);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion Get My Team Info

    }
}
