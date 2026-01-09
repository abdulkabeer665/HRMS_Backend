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
    public class EmployeeController : ControllerBase
    {

        #region "Declaration"

        private static string SP_GetAllEmployees = "[dbo].[SP_GetAllEmployees]";
        private static string SP_GetAllEmployeesDD = "[dbo].[SP_GetAllEmployeesDD]";

        #endregion

        #region "Get All Employees Dropdown"

        /// <summary>
        /// This API will return all Employees for dropdown
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet("GetAllEmployeesDD")]
        [Authorize]
        public async Task<IActionResult> GetAllEmployeesDD()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = await DataLogic.GetDataAsync(SP_GetAllEmployeesDD);
                return Ok(dt);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion

        #region "Get All Employees"

        /// <summary>
        /// This API will return all Employees
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet("GetAllEmployees")]
        [Authorize]
        public async Task<IActionResult> GetAllEmployees()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = await DataLogic.GetDataAsync(SP_GetAllEmployees);
                string profilePhotoDirectory = GenericFunctions.GetFileLocation();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string? fileName = dt.Rows[i]["FileName"].ToString();
                    if (fileName != null && fileName != "")
                    {
                        dt.Rows[i]["FileName"] = profilePhotoDirectory + "\\" + dt.Rows[i]["FileName"].ToString();
                    }
                    else
                    {
                        dt.Rows[i]["FileName"] = profilePhotoDirectory + "\\" + "user.png";
                    };
                    
                };
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
