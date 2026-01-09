using HRMS_Backend.BAL;
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
    public class RolesController : ControllerBase
    {

        #region "Declaration"

        private static string SP_GetAllRoles = "[dbo].[SP_GetAllRoles]";
        private static string SP_GetAllRolesDD = "[dbo].[SP_GetAllRolesDD]";
        private static string SP_GetRightsForMenusAgainstRoleID = "[dbo].[SP_GetRightsForMenusAgainstRoleID]";
        private static string SP_InsertUpdateDeleteRole = "[dbo].[SP_InsertUpdateDeleteRole]";

        #endregion

        #region "Get All Roles"

        /// <summary>
        /// Get Leaves availed against leave type and emp id
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet("GetAllRoles")]
        [Authorize]
        public async Task<IActionResult> GetAllRoles()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = await DataLogic.GetAllRoles(SP_GetAllRoles);
                return Ok(dt);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion

        #region "Get All Roles Dropdown"

        /// <summary>
        /// Get All Roles Dropdown
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet("GetAllRolesDD")]
        [Authorize]
        public async Task<IActionResult> GetAllRolesDD()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = await DataLogic.GetDataAsync(SP_GetAllRolesDD);
                return Ok(dt);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion

        #region Get Rights Against Role ID

        /// <summary>
        /// This API will return all the rights of a selected Role
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("GetRoleInfoByRoleID")]
        [Authorize]
        public async Task<IActionResult> GetRoleInfoByRoleID([FromBody] RoleReqParams roleReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = await DataLogic.GetRoleInfoByRoleID(roleReqParams, SP_GetRightsForMenusAgainstRoleID);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("ErrorMessage"))
                    {
                        msg.message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        ds.Tables[0].TableName = "MasterMenu";
                        ds.Tables[1].TableName = "ChildMenu";
                        ds.Tables[2].TableName = "Options";
                        ds.Tables[3].TableName = "MenuRights";
                        return Ok(ds);
                    }
                }
                else
                {
                    return Ok(ds);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Insert Role

        /// <summary>
        /// This API will use to insert an Role
        /// </summary>
        /// /// <param name="insertRoleReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertRole")]
        [Authorize]
        public async Task<IActionResult> InsertRole([FromBody] InsertRoleReqParams insertRoleReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = await DataLogic.InsertRole(insertRoleReqParams, SP_InsertUpdateDeleteRole);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = dt.Rows[0]["Message"].ToString();
                        msg.status = dt.Rows[0]["Status"].ToString();
                        return Ok(msg);
                    }
                }
                else
                {
                    return Ok(dt);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Update Role

        /// <summary>
        /// This API will use to Update right for Role
        /// </summary>
        /// /// <param name="updateRoleReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateRole")]
        [Authorize]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleReqParams updateRoleReqParams)
        {
            Message msg = new Message();
            try
            {

                DataTable dtMain = new DataTable();
                dtMain.Columns.Add("RoleAsingmentID"); dtMain.Columns.Add("RoleID"); dtMain.Columns.Add("Value");
                if (updateRoleReqParams.roleAssignOptions_list != null)
                {
                    dtMain = ListIntoDataTable.ToDataTable(updateRoleReqParams.roleAssignOptions_list);
                }

                updateRoleReqParams.RoleID = dtMain.Rows[0]["RoleID"].ToString();
                updateRoleReqParams.Delete = 0;
                updateRoleReqParams.UserInfo = 0;

                DataTable dt = await DataLogic.UpdateRole(dtMain, updateRoleReqParams, SP_InsertUpdateDeleteRole);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = dt.Rows[0]["Message"].ToString();
                        msg.status = dt.Rows[0]["Status"].ToString();
                        return Ok(msg);
                    }
                }
                else
                {
                    return Ok(dt);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Update Role Info

        /// <summary>
        /// This API will use to Update info for Role
        /// </summary>
        /// /// <param name="updateRoleInfoReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateRoleInfo")]
        [Authorize]
        public async Task<IActionResult> UpdateRoleInfo([FromBody] UpdateRoleInfoReqParams updateRoleInfoReqParams)
        {
            Message msg = new Message();
            try
            {
                updateRoleInfoReqParams.UserInfo = 1;
                updateRoleInfoReqParams.Delete = 0;
                DataTable dt = await DataLogic.UpdateRoleInfo(updateRoleInfoReqParams, SP_InsertUpdateDeleteRole);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = dt.Rows[0]["Message"].ToString();
                        msg.status = dt.Rows[0]["Status"].ToString();
                        return Ok(msg);
                    }
                }
                else
                {
                    return Ok(dt);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Delete App Role

        /// <summary>
        /// This API will use to delete an Role
        /// </summary>
        /// /// <param name="deleteRoleReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteRole")]
        [Authorize]
        public async Task<IActionResult> DeleteRole([FromBody] DeleteRoleReqParams deleteRoleReqParams)
        {
            Message msg = new Message();
            try
            {
                deleteRoleReqParams.Delete = 1;
                DataTable dt = await DataLogic.DeleteRole(deleteRoleReqParams, SP_InsertUpdateDeleteRole);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = dt.Rows[0]["Message"].ToString();
                        msg.status = dt.Rows[0]["Status"].ToString();
                        return Ok(msg);
                    }
                }
                else
                {
                    return Ok(dt);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

    }
}
