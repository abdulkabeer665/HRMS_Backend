using HRMS_Backend.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.RegularExpressions;
using static HRMS_Backend.BAL.RequestParameters;
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
        private static string SP_ChangePassword = "[dbo].[SP_ChangePassword]";
        private static string SP_UpdateEmployeeInfo = "[dbo].[SP_UpdateEmployeeInfo]";

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

        #region Change Password

        /// <summary>
        /// This API will use to change password
        /// </summary>
        /// /// <param name="changePassReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword changePassReqParams)
        {
            Message msg = new Message();
            try
            {

                if (changePassReqParams.LoginName?.Trim() == "")
                {
                    msg.message = "Login Name is required.";
                    msg.status = "401";
                    return Ok(msg);
                }
                if (changePassReqParams.NewPassword?.Trim() == "")
                {
                    msg.message = "New Password is required.";
                    msg.status = "401";
                    return Ok(msg);
                }

                if (changePassReqParams.OldPassword?.Trim() == "")
                {
                    msg.message = "Old Password is required.";
                    msg.status = "401";
                    return Ok(msg);
                }

                if (string.IsNullOrWhiteSpace(changePassReqParams.NewPassword))
                {
                    msg.message = "New Password is not valid.";
                    msg.status = "401";
                    return Ok(msg);
                }


                if (changePassReqParams.NewPassword.Length < 8)      // Check length
                {
                    msg.message = "New Password length must be greater than 8.";
                    msg.status = "401";
                    return Ok(msg);
                }

                bool hasUpperCase = Regex.IsMatch(changePassReqParams.NewPassword, "[A-Z]");     // Check for at least one uppercase letter
                if (!hasUpperCase)
                {
                    msg.message = "New Password must have a capital character.";
                    msg.status = "401";
                    return Ok(msg);
                }

                bool hasSpecialChar = Regex.IsMatch(changePassReqParams.NewPassword, "[^a-zA-Z0-9]");    // Check for at least one special character
                if (!hasSpecialChar)
                {
                    msg.message = "New Password must have a special character.";
                    msg.status = "401";
                    return Ok(msg);
                }

                changePassReqParams.OldPassword = EncryptDecryptPassword.EncryptQueryString(changePassReqParams.LoginName?.Trim() + "|" + changePassReqParams.OldPassword.Trim());
                changePassReqParams.NewPassword = EncryptDecryptPassword.EncryptQueryString(changePassReqParams.LoginName?.Trim() + "|" + changePassReqParams.NewPassword.Trim());

                DataTable dt = await DataLogic.VerifyOldPassword(changePassReqParams, SP_ChangePassword);
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

        #region Update Info

        /// <summary>
        /// This API will use to Update Info
        /// </summary>
        /// /// <param name="updateInfoReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateInfo")]
        [Authorize]
        public async Task<IActionResult> UpdateInfo([FromForm] UpdateInfoReqParams updateInfoReqParams)
        {
            Message msg = new Message();
            try
            {
                var profilePhotoUploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Employee", "ProfilePhoto");
                if (!Directory.Exists(profilePhotoUploadDirectory))
                {
                    Directory.CreateDirectory(profilePhotoUploadDirectory);
                };
                string? fileName = Path.GetFileName(updateInfoReqParams.EmployeePhoto?.FileName);
                string? filePath = profilePhotoUploadDirectory;
                string fileExtension = Path.GetExtension(fileName).ToLower();

                DataTable dt = await DataLogic.UpdateInfo(updateInfoReqParams, updateInfoReqParams.EmpID + fileExtension, filePath, fileExtension, SP_UpdateEmployeeInfo);
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

                        string finalFilePath = Path.Combine(profilePhotoUploadDirectory, updateInfoReqParams.EmpID + fileExtension);
                        if (System.IO.File.Exists(finalFilePath))
                        {
                            System.IO.File.Delete(finalFilePath);
                        }

                        using (var stream = new FileStream(finalFilePath, FileMode.Create))
                        {
                            await updateInfoReqParams.EmployeePhoto.CopyToAsync(stream);
                        }
                        return Ok(new
                        {
                            msg,
                            employeePhoto = Path.Combine("Uploads", "Employee", "ProfilePhoto", updateInfoReqParams.EmpID + fileExtension)
                        });
                    };
                }
                else
                {
                    return Ok(dt);
                };
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            };
        }

        #endregion

    }
}
