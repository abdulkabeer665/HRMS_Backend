using HRMS_Backend.DAL;
using HRMS_Backend.Model;
using HRMS_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using static HRMS_Backend.BAL.RequestParameters;
using static HRMS_Backend.BAL.ResponseParameters;

namespace HRMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {

        #region "Declaration"

        public readonly static string SP_UserLogin = "[dbo].[SP_UserLogin]";
        public readonly static string UpdateRefreshToken_SP = "[dbo].[UpdateRefreshToken]";
        public readonly static string SP_GetUserByID = "[dbo].[GetUserByID]";

        #endregion

        #region User Login

        /// <summary>
        /// Login API
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        /// 

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] Loginparam loginParam)
        {
            Message msg = new Message();
            LoginRes loginRes;
            try
            {
                var test = EncryptDecryptPassword.DecryptQueryString(loginParam.Password);
                loginParam.Password = EncryptDecryptPassword.EncryptQueryString(loginParam.LoginName + "|" + loginParam.Password);

                DataTable dt = DataLogic.LoginDetails(loginParam, SP_UserLogin);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "400";
                        return BadRequest(msg);
                    }
                    else
                    {
                        var loginNamefromDB = EncryptDecryptPassword.DecryptQueryString(dt.Rows[0]["Password"].ToString()).Split("|");
                        var loginName = dt.Rows[0]["LoginName"].ToString();
                        var userName = dt.Rows[0]["UserName"].ToString();
                        var roleID = dt.Rows[0]["RoleID"].ToString();
                        var role = dt.Rows[0]["Role"].ToString();
                        var status = dt.Rows[0]["Status"].ToString();

                        #region Return Parameters

                        if (loginNamefromDB[0] == loginParam.LoginName && status == "200")
                        {

                            #region JWT Authentication

                            var tokenString = JWTBuilder.Generation(loginName, userName, roleID, status);

                            var refreshToken = JWTBuilder.GenerateRefreshToken();
                            DataTable dt2 = DataLogic.UpdateRefreshToken(loginParam.LoginName, refreshToken, UpdateRefreshToken_SP);

                            #endregion


                            loginRes = new LoginRes();

                            loginRes.UserName = userName;
                            loginRes.LoginName = loginName;
                            loginRes.RoleID = roleID;
                            loginRes.EmpID = dt.Rows[0]["empID"].ToString();
                            loginRes.Role = role;
                            loginRes.Designation = dt.Rows[0]["designation"].ToString();
                            loginRes.Status = status;
                            loginRes.Message = "User authenticated";
                            loginRes.RefreshToken = dt.Rows[0]["refreshToken"].ToString();
                            loginRes.RefreshTokenNew = refreshToken;
                            loginRes.Token = tokenString.Item1.ToString();
                            //loginRes.ValidTill = tokenString.Item2.ToString();
                            loginRes.ValidTill = tokenString.Item2?.ToUniversalTime().ToString("o");

                            //DataTable auditDT = DataLogic.InsertAuditLogs("Login", 1, "Authenticated", loginParam.LoginName, "dbo.AppUsers");

                            return Ok(loginRes);
                        }
                        else if (status == "401")
                        {
                            loginRes = new LoginRes();

                            loginRes.UserName = "";
                            loginRes.LoginName = "";
                            loginRes.RoleID = "";
                            loginRes.Role = "";
                            loginRes.Designation = "";
                            loginRes.EmpID = "";
                            loginRes.Status = status;
                            loginRes.Message = "User not authenticated";
                            loginRes.RefreshToken = "";
                            loginRes.RefreshTokenNew = "";
                            loginRes.Token = "";
                            loginRes.ValidTill = "";
                            //DataTable auditDT = DataLogic.InsertAuditLogs("Login", 1, "Not Authenticated", loginParam.LoginName, "dbo.AppUsers");

                            return Unauthorized(loginRes);
                        }
                        else
                        {
                            loginRes = new LoginRes();

                            loginRes.UserName = "";
                            loginRes.LoginName = "";
                            loginRes.RoleID = "";
                            loginRes.Role = "";
                            loginRes.Designation = "";
                            loginRes.EmpID = "";
                            loginRes.Status = status;
                            loginRes.Message = "User not found";
                            loginRes.RefreshToken = "";
                            loginRes.RefreshTokenNew = "";
                            loginRes.Token = "";
                            loginRes.ValidTill = "";

                            //DataTable auditDT = DataLogic.InsertAuditLogs("Login", 1, "Not Found", loginParam.LoginName, "dbo.AppUsers");

                            return Unauthorized(loginRes);
                        }

                        #endregion

                    }
                }
                else
                {
                    loginRes = new LoginRes();

                    loginRes.UserName = "";
                    loginRes.LoginName = "";
                    loginRes.RoleID = "";
                    loginRes.Role = "";
                    loginRes.Designation = "";
                    loginRes.EmpID = "";
                    loginRes.Status = "400";
                    loginRes.Message = "User not found";
                    loginRes.RefreshToken = "";
                    loginRes.RefreshTokenNew = "";
                    loginRes.Token = "";
                    loginRes.ValidTill = "";
                    return Unauthorized(loginRes);
                }

            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion

        #region Refresh Token
        /// <summary>
        /// Refreshing Token when Last token is valid
        /// </summary>
        /// <param name="refreshTokenRequest"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                LoginRes loginRes = new LoginRes();
                var principal = JWTBuilder.GetClaimsFromExpiredToken(refreshTokenRequest.JWTToken);
                var loginName_ = principal?.Claims?.SingleOrDefault(p => p.Type == UserClaimParameters.LOGINNAME.ToString())?.Value;
                DataTable dt = DataLogic.GetUserByID(loginName_.ToString(), SP_GetUserByID);
                var status = dt.Rows[0]["Status"].ToString();
                if (dt.Columns.Contains("ErrorMessage"))
                {
                    return Ok(dt);
                }
                else
                {

                    #region Return Params

                    if (status == "200")
                    {

                        #region Refreshing Token

                        var refreshToken = dt.Rows[0]["RefreshToken"].ToString();
                        if (refreshToken != refreshTokenRequest.RefreshToken)
                            throw new SecurityTokenException("Invalid refresh token");
                        var loginName = dt.Rows[0]["LoginName"].ToString();
                        var userName = dt.Rows[0]["UserName"].ToString();
                        var roleID = dt.Rows[0]["RoleID"].ToString();
                        status = dt.Rows[0]["Status"].ToString();
                        var newJwtToken = JWTBuilder.Generation(loginName, userName, roleID, status);
                        var newRefreshToken = JWTBuilder.GenerateRefreshToken();

                        #endregion

                        DataTable dt2 = DataLogic.UpdateRefreshToken(loginName, newRefreshToken, UpdateRefreshToken_SP);
                        DataSet ds2 = DataLogic.GetDetails(loginName, SP_GetUserByID);


                        loginRes.UserName = userName;
                        loginRes.LoginName = loginName;
                        loginRes.RoleID = roleID;
                        loginRes.Status = status;
                        loginRes.Message = "User authenticated";
                        loginRes.RefreshToken = dt.Rows[0]["refreshToken"].ToString();
                        loginRes.RefreshTokenNew = newRefreshToken;
                        loginRes.Token = newJwtToken.Item1.ToString();
                        //loginRes.ValidTill = newJwtToken.Item2?.ToUniversalTime().ToString();
                        loginRes.ValidTill = newJwtToken.Item2
    ?.ToUniversalTime()
    .ToString("yyyy-MM-ddTHH:mm:ssZ");



                        return Ok(loginRes);
                    }
                    else
                    {
                        loginRes.UserName = "";
                        loginRes.LoginName = "";
                        loginRes.RoleID = "";
                        loginRes.Status = status;
                        loginRes.Message = "User not found";
                        loginRes.RefreshToken = "";
                        loginRes.RefreshTokenNew = "";
                        loginRes.Token = "";
                        loginRes.ValidTill = "";

                        return Unauthorized(loginRes);
                    }

                    #endregion

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

    }
}
