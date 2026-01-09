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
    public class DepartmentController : ControllerBase
    {

        #region "Declaration"

        private static string SP_GetAllDepartments = "[dbo].[SP_GetAllDepartments]";
        private static string SP_GetAllDepartmentsDD = "[dbo].[SP_GetAllDepartmentsDD]";

        #endregion

        #region "Get All Departments"

        /// <summary>
        /// This API will return all departments
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet("GetAllDepartments")]
        [Authorize]
        public async Task<IActionResult> GetAllDepartments()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = await DataLogic.GetDataAsync(SP_GetAllDepartments);
                return Ok(dt);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion

        #region "Get All Departments Dropdown"

        /// <summary>
        /// This API will return all departments for dropdown
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet("GetAllDepartmentsDD")]
        [Authorize]
        public async Task<IActionResult> GetAllDepartmentsDD()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = await DataLogic.GetDataAsync(SP_GetAllDepartmentsDD);
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
