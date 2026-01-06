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
    public class MenuController : ControllerBase
    {
        #region "Declaration"

        private static string SP_GetMenusAgainstRoleID = "[dbo].[SP_GetMenusAgainstRoleID]";
        private static string SP_GetOptionsAgainstRoleAndMenuID = "[dbo].[SP_GetOptionsAgainstRoleAndMenuID]";

        #endregion

        #region "Get All Menus and their respective Menu Items"
        /// <summary>
        /// This endpoint will use to return all the active and allowed menus to the ROLE
        /// </summary>
        /// <param name="menuReqParams"></param>
        [HttpPost("GetAllMenus")]
        [Authorize]
        public IActionResult GetAllMenus([FromBody] MenuReqParams menuReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllMenus(menuReqParams, SP_GetMenusAgainstRoleID);
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
                        ds.Tables[0].TableName = "ParentMenu";
                        ds.Tables[1].TableName = "ChildMenu";
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

        #region "Get All Options against Role and Menu ID"
        /// <summary>
        /// This endpoint will use to get Add, Edit, Delete and View option against MenuID and RoleID
        /// </summary>
        /// <param name="menuReqParams"></param>
        [HttpPost("GetMenuOptions")]
        [Authorize]
        public IActionResult GetMenuOptions([FromBody] MenuReqParams menuReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetMenuOptions(menuReqParams, SP_GetOptionsAgainstRoleAndMenuID);
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
                        return Ok(dt);
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