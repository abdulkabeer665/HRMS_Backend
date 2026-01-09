using HRMS_Backend.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static HRMS_Backend.BAL.ResponseParameters;

namespace HRMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : ControllerBase
    {

        #region "Declaration"

        private static string SP_GetAllDesignationsDD = "[dbo].[SP_GetAllDesignationsDD]";

        #endregion

        #region "Get All Designation Dropdown"

        /// <summary>
        /// This API will return all Designations for dropdown
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet("GetAllDesignationsDD")]
        [Authorize]
        public async Task<IActionResult> GetAllDesignationsDD()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = await DataLogic.GetDataAsync(SP_GetAllDesignationsDD);
                return Ok(dt);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion

    }
}
