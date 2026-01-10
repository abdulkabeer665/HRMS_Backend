using HRMS_Backend.DAL;
using HRMS_Backend.Services;
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
    public class DashboardController : ControllerBase
    {

        #region "Declaration"

        public readonly static string SP_GetEmployessCount = "[dbo].[SP_GetEmployessCount]";
        public readonly static string SP_GetDashboardMenusAgainstRoleID = "[dbo].[SP_GetDashboardMenusAgainstRoleID]";
        public readonly static string SP_GetLeavesCountAgainstEmpID = "[dbo].[SP_GetLeavesCountAgainstEmpID]";
        public readonly static string SP_GetEmployeeInfoAgainstEmpID = "[dbo].[SP_GetEmployeeInfoAgainstEmpID]";
        public readonly static string SP_GetLeavesEntitledAndAvailedAgainstEmpID = "[dbo].[SP_GetLeavesEntitledAndAvailedAgainstEmpID]";
        public readonly static string SP_GetPendingLeavesCountAgainstRoleID = "[dbo].[SP_GetPendingLeavesCountAgainstRoleID]";

        #endregion

        #region Employees Count

        /// <summary>
        /// Employees Count
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet("EmployeesCount")]
        [AllowAnonymous]
        public async Task<IActionResult> EmployeesCount()
        {
            Message msg = new Message();
            LoginRes loginRes;
            try
            {
                DataTable dt = await DataLogic.GetDataAsync(SP_GetEmployessCount);
                return Ok(dt);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion Employees Count

        #region Dashboard Menus

        /// <summary>
        /// Get Dashboard Menus By passing the RoleID
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost("DashboardMenusAgainstRoleID")]
        [Authorize]
        public async Task<IActionResult> DashboardMenusAgainstRoleID([FromBody] UserReqParams userReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = await DataLogic.DashboardMenusAgainstRoleID(userReqParams, SP_GetDashboardMenusAgainstRoleID);
                return Ok(dt);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion Dashboard Menus

        #region Leaves Count for Employee

        /// <summary>
        /// Get Leaves Count against EmpID
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost("LeavesCountAgainstEmpID")]
        [Authorize]
        public async Task<IActionResult> LeavesCountAgainstEmpID([FromBody] UserReqParams userReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = await DataLogic.LeavesCountAgainstEmpID(userReqParams, SP_GetLeavesCountAgainstEmpID);
                return Ok(ds);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion Leaves Count

        #region Employee Info

        /// <summary>
        /// Get Employee Info against EmpID
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost("GetEmployeeInfoAgainstEmpID")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeInfoAgainstEmpID([FromBody] EmpIDReqParams empIDReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = await DataLogic.GetEmployeeInfoAgainstEmpID(empIDReqParams, SP_GetEmployeeInfoAgainstEmpID);
                string? fileName = dt.Rows[0]["FileName"].ToString();
                string profilePhotoDirectory = GenericFunctions.GetFileLocation();
                if (fileName != null && fileName != "")
                {
                    dt.Rows[0]["FileName"] = profilePhotoDirectory + "\\" + dt.Rows[0]["FileName"].ToString();
                }
                else
                {
                    dt.Rows[0]["FileName"] = profilePhotoDirectory + "\\" + "user.png";
                }
                ;
                return Ok(dt);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion Dashboard Menus

        #region Leaves Entitled and Availed Against EmpID

        /// <summary>
        /// Get Leaves Entitled and Availed against EmpID
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost("GetLeavesEntitledAvailedAgainstEmpID")]
        [Authorize]
        public async Task<IActionResult> GetLeavesEntitledAvailedAgainstEmpID([FromBody] EmpIDReqParams empIDReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = await DataLogic.GetLeavesEntitledAvailedAgainstEmpID(empIDReqParams, SP_GetLeavesEntitledAndAvailedAgainstEmpID);
                DataTable dt = new DataTable();
                dt.Columns.Add("LeaveTitle", typeof(string));
                dt.Columns.Add("TotalLeaves", typeof(int));
                dt.Columns.Add("LeavesAvailed", typeof(int));

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            dt.Rows.Add(
                                row["LeaveTitle"].ToString(),
                                Convert.ToInt32(row["TotalLeaves"]),
                                Convert.ToInt32(row["LeavesAvailed"])
                            );
                        }
                    }
                }

                return Ok(dt);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion Leaves Entitled and Availed Against EmpID

        #region Pending Leaves Count

        /// <summary>
        /// Get Pending Leaves Count against Role ID
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost("PendingLeavesCountAgainstRoleID")]
        [Authorize]
        public async Task<IActionResult> PendingLeavesCountAgainstRoleID([FromBody] RoleIDReqParams roleIDReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = await DataLogic.GetPendingLeavesAgainstRoleID(roleIDReqParams, SP_GetPendingLeavesCountAgainstRoleID);
                DataTable dt = new DataTable();
                dt.Columns.Add("PendingLeavesCount", typeof(string));

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            dt.Rows.Add(Convert.ToInt32(row["PendingLeavesCount"]));
                        }
                    }
                }

                return Ok(dt);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion Leaves Count

    }
}
