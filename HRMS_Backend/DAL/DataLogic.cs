using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Security.Cryptography;
using static HRMS_Backend.BAL.RequestParameters;

namespace HRMS_Backend.DAL
{
    public class DataLogic
    {

        #region "Login & Password Related Logics"

        #region LoginDetails

        public static DataTable LoginDetails(Loginparam loginParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LoginName", loginParam.LoginName),
                new SqlParameter ("@Pass", loginParam.Password)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region GeneratePasswordResetToken

        public static DataTable GeneratePasswordResetToken(GeneratePasswordTokenParam gptP, string StoreProcedure)
        {
            DbReports CGD = new DbReports();

            var passwordResetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            var datetime = DateTime.Now;

            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@loginName", gptP.LoginName),
                new SqlParameter ("@PassowrdResetTokenDate", datetime),
                new SqlParameter ("@PasswordResetToken", passwordResetToken)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region ForGot Password

        public static DataTable ForgotPass(ForgotPassword forgotPassword, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LoginName", forgotPassword.LoginName)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        public static DataTable UpdateUserPassword(ForgotPassword forgotPassword, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LoginName", forgotPassword.LoginName),
                new SqlParameter ("@Password", forgotPassword.NewPassword)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Password Reset Token Date

        public static DataTable GetPasswordResetTokenDate(ForgotPassword forgotPassword, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LoginName", forgotPassword.LoginName)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region UpdateRefreshToken

        public static DataTable UpdateRefreshToken(string loginName, string refreshToken, string StoredProcedure)
        {
            try
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@LoginName",loginName),
                    new SqlParameter ("@RefreshToken",refreshToken)
                };

                DataTable dt = CGD.DTWithParam(StoredProcedure, sqlParameters, 1);
                return dt;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                return new DataTable(errorMessage);
            }
        }

        #endregion

        #region Get User By ID

        public static DataTable GetUserByID(string loginName, string StoreProcedure)
        {
            try
            {
                DbReports CGD = new DbReports();

                SqlParameter[] sqlParameters =
                {
                    new SqlParameter("@LoginName",loginName),
                };
                return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
            }
            catch (Exception ex)
            {
                return new DataTable(ex.Message);
            }
        }

        #endregion

        #region LoginDetails

        public static DataSet GetDetails(string loginName, string StoredProcedure)
        {
            try
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@LoginName", loginName)
                };

                DataSet ds = CGD.DSWithParam(StoredProcedure, sqlParameters, 1);
                return ds;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                return new DataSet(errorMessage);
            }
        }

        #endregion

        #region VerifyOldPassword

        public static DataTable VerifyOldPassword(ChangePassword changePassword, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LoginName", changePassword.LoginName),
                new SqlParameter ("@OldPassword", changePassword.OldPassword),
                new SqlParameter ("@NewPassword", changePassword.NewPassword)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region "Dashboard Related Logics"

        #region Get Data Async

        public static async Task<DataTable> GetDataAsync(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                //new SqlParameter ("@LoginName", loginParam.LoginName),
                //new SqlParameter ("@Pass", loginParam.Password)
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Dashboard Menus Against Role ID

        public static async Task<DataTable> DashboardMenusAgainstRoleID(UserReqParams userReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@RoleID", userReqParams.RoleID),
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Leaves Count against Emp ID

        public static async Task<DataSet> LeavesCountAgainstEmpID(UserReqParams userReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@EmpID", userReqParams.EmpID),
            };
            return await CGD.DSWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion Leaves Count against Emp ID

        #region Get Employee Info Against Emp ID

        public static async Task<DataTable> GetEmployeeInfoAgainstEmpID(EmpIDReqParams empIDReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@EmpID", empIDReqParams.EmpID),
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Leaves Entitled Availed Against EmpID

        public static async Task<DataSet> GetLeavesEntitledAvailedAgainstEmpID(EmpIDReqParams empIDReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@EmpID", empIDReqParams.EmpID),
            };
            return await CGD.DSWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion Get Leaves Entitled Availed Against EmpID

        #region Get Pending Leaves Against RoleID

        public static async Task<DataSet> GetPendingLeavesAgainstRoleID(RoleIDReqParams roleIDReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@RoleID", roleIDReqParams.RoleID),
                new SqlParameter ("@ReportingManagerID", roleIDReqParams.ReportingManagerID),
            };
            return await CGD.DSWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion Get Pending Leaves Against RoleID

        #endregion 

        #region Leaves Related Logics

        #region Get Leaves Availed Against Leave Type

        public static async Task<DataTable> GetLeaveAvailedAgainstLeaveType(LeaveReqParams leaveReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LeaveType", leaveReqParams.LeaveType),
                new SqlParameter ("@EmpID", leaveReqParams.EmpID),
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion Get Leaves Availed Against Leave Type

        #region Pending Leaves

        public static async Task<DataTable> PendingLeaves(PendingLeavesReqParams pendingLeavesReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LeaveType", pendingLeavesReqParams.LeaveType),
                new SqlParameter ("@RoleID", pendingLeavesReqParams.RoleID),
                new SqlParameter ("@ReportingManagerID", pendingLeavesReqParams.ReportingManagerID),
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion Pending Leaves

        #region Check Leave Days Remaining

        public static async Task<DataTable> CheckLeaveDaysRemaining(CasualLeaveReqParams casualLeaveReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@EmpID", casualLeaveReqParams.EmpID),
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion Check Leave Days Remaining

        #region Check Leave Dates Exists

        public static async Task<DataTable> CheckLeaveDatesExists(CasualLeaveReqParams casualLeaveReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@FromDate", casualLeaveReqParams.FromDate),
                new SqlParameter ("@ToDate", casualLeaveReqParams.ToDate),
                new SqlParameter ("@EmpID", casualLeaveReqParams.EmpID),
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion Check Leave Dates Exists

        #region Delete File From DB

        public static async Task<DataTable> DeleteFileFromDB(string refNo, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@RefNo", refNo)
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion Delete File From DB

        #region Insert Casual Leave

        public static async Task<DataTable> InsertCasualLeave(CasualLeaveReqParams casualLeaveReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LeaveType", casualLeaveReqParams.LeaveType),
                new SqlParameter ("@FromDate", casualLeaveReqParams.FromDate),
                new SqlParameter ("@ToDate", casualLeaveReqParams.ToDate),
                new SqlParameter ("@EmpID", casualLeaveReqParams.EmpID),
                new SqlParameter ("@RequestDate", casualLeaveReqParams.RequestDate),
                new SqlParameter ("@Reason", casualLeaveReqParams.Reason),
                new SqlParameter ("@ReportingManagerID", casualLeaveReqParams.ReportingManagerID),
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion Insert Casual Leave

        #region Update Casual Leave

        public static async Task<DataTable> UpdateCasualLeave(CasualLeaveReqParams casualLeaveReqParams, string StoreProcedure)
        {
            DbReports CGD = new();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LeaveID", casualLeaveReqParams.LeaveID),
                new SqlParameter ("@FromDate", casualLeaveReqParams.FromDate),
                new SqlParameter ("@ToDate", casualLeaveReqParams.ToDate),
                new SqlParameter ("@ManagerApproved", casualLeaveReqParams.ManagerApproved),
                new SqlParameter ("@ManagerApprovedDate", casualLeaveReqParams.ManagerApprovedDate),
                new SqlParameter ("@ManagerRemarks", casualLeaveReqParams.ManagerRemarks),
                new SqlParameter ("@HRID", casualLeaveReqParams.HRID),
                new SqlParameter ("@HRApproved", casualLeaveReqParams.HRApproved),
                new SqlParameter ("@HRApprovedDate", casualLeaveReqParams.HRApprovedDate),
                new SqlParameter ("@HRRemarks", casualLeaveReqParams.HRRemarks),
                new SqlParameter ("@LeaveStatus", casualLeaveReqParams.LeaveStatus),
                new SqlParameter ("@PendingFrom", casualLeaveReqParams.PendingFrom),          
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion Update Casual Leave

        #region Get Leave Info against Ref No

        public static async Task<DataTable> GetLeaveInfoAgainstRefNo(LeaveInfoReqParams leaveInfoReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LeaveType", leaveInfoReqParams.LeaveType),
                new SqlParameter ("@RefNo", leaveInfoReqParams.RefNo),
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion Get Leave Info against Ref No

        #region Get Emp ID against Designation

        public static async Task<DataTable> GetEmpIDAgainstDesignation(string designation, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@Designation", designation),
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }
        #endregion

        #region Insert Sick Leave

        public static async Task<DataTable> InsertSickLeave(SickLeaveReqParams sickLeaveReqParams, string fileName, string filePath, string fileExtension, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LeaveType", sickLeaveReqParams.LeaveType),
                new SqlParameter ("@FromDate", sickLeaveReqParams.FromDate),
                new SqlParameter ("@ToDate", sickLeaveReqParams.ToDate),
                new SqlParameter ("@EmpID", sickLeaveReqParams.EmpID),
                new SqlParameter ("@RequestDate", sickLeaveReqParams.RequestDate),
                new SqlParameter ("@FileName", fileName),
                new SqlParameter ("@FilePath", filePath),
                new SqlParameter ("@FileExtension", fileExtension),
                new SqlParameter ("@Reason", sickLeaveReqParams.Reason),
                new SqlParameter ("@ReportingManagerID", sickLeaveReqParams.ReportingManagerID),      
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion Insert Sick Leave

        #region Update Sick Leave Without Attachment

        public static async Task<DataTable> UpdateSickLeaveWithoutAttachment(SickLeaveReqParams sickLeaveReqParams, string StoreProcedure)
        {
            DbReports CGD = new();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LeaveID", sickLeaveReqParams.LeaveID),
                new SqlParameter ("@RefNo", sickLeaveReqParams.RefNo),
                new SqlParameter ("@FromDate", sickLeaveReqParams.FromDate),
                new SqlParameter ("@ToDate", sickLeaveReqParams.ToDate),
                new SqlParameter ("@ManagerApproved", sickLeaveReqParams.ManagerApproved),
                new SqlParameter ("@ManagerApprovedDate", sickLeaveReqParams.ManagerApprovedDate),
                new SqlParameter ("@ManagerRemarks", sickLeaveReqParams.ManagerRemarks),
                new SqlParameter ("@HRID", sickLeaveReqParams.HRID),
                new SqlParameter ("@HRApproved", sickLeaveReqParams.HRApproved),
                new SqlParameter ("@HRApprovedDate", sickLeaveReqParams.HRApprovedDate),
                new SqlParameter ("@HRRemarks", sickLeaveReqParams.HRRemarks),
                new SqlParameter ("@LeaveStatus", sickLeaveReqParams.LeaveStatus),
                new SqlParameter ("@PendingFrom", sickLeaveReqParams.PendingFrom),                        
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion Update Sick Leave Without Attachment

        #region Update Sick Leave With Attachment

        public static async Task<DataTable> UpdateSickLeaveWithAttachment(SickLeaveReqParams sickLeaveReqParams, string fileName, string filePath, string fileExtension, string StoreProcedure)
        {
            DbReports CGD = new();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LeaveID", sickLeaveReqParams.LeaveID),
                new SqlParameter ("@RefNo", sickLeaveReqParams.RefNo),
                new SqlParameter ("@FromDate", sickLeaveReqParams.FromDate),
                new SqlParameter ("@ToDate", sickLeaveReqParams.ToDate),
                new SqlParameter ("@ManagerApproved", sickLeaveReqParams.ManagerApproved),
                new SqlParameter ("@ManagerApprovedDate", sickLeaveReqParams.ManagerApprovedDate),
                new SqlParameter ("@ManagerRemarks", sickLeaveReqParams.ManagerRemarks),
                new SqlParameter ("@HRID", sickLeaveReqParams.HRID),
                new SqlParameter ("@FileName", fileName),
                new SqlParameter ("@FilePath", filePath),
                new SqlParameter ("@FileExtension", fileExtension),
                new SqlParameter ("@HRApproved", sickLeaveReqParams.HRApproved),
                new SqlParameter ("@HRApprovedDate", sickLeaveReqParams.HRApprovedDate),
                new SqlParameter ("@HRRemarks", sickLeaveReqParams.HRRemarks),
                new SqlParameter ("@LeaveStatus", sickLeaveReqParams.LeaveStatus),
                new SqlParameter ("@PendingFrom", sickLeaveReqParams.PendingFrom),
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion Update Sick Leave With Attachment

        #endregion Leaves Related Logics

        #region "Menus Logics"

        #region "Get All Menus"

        public static DataSet GetAllMenus(MenuReqParams menuReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@RoleID", menuReqParams.RoleID)
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region "Get All Options Against MenuID and Role"

        public static DataTable GetMenuOptions(MenuReqParams menuReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@RoleID", menuReqParams.RoleID),
                new SqlParameter ("@MenuID", menuReqParams.MenuID)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region "My Team Logics"

        #region Get My Team

        public static async Task<DataTable> GetMyTeamInfo(ReportingManagerReqParams reportingManagerReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@ReportingManagerID", reportingManagerReqParams.ReportingManagerID),
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion Get My Tem

        #endregion

        #region "Roles Logics"

        #region "Get All Roles"

        public static async Task<DataTable> GetAllRoles(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = { };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Role Info By RoleID

        public static async Task<DataSet> GetRoleInfoByRoleID(RoleReqParams roleReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@RoleID", roleReqParams.RoleID)
            };
            return await CGD.DSWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Insert Role

        public static async Task<DataTable> InsertRole(InsertRoleReqParams insertRoleReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@Description", insertRoleReqParams.Description),
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Role

        public static async Task<DataTable> UpdateRole(DataTable dt, UpdateRoleReqParams updateRoleReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@RoleAssignOptions_DTable", dt),
                new SqlParameter ("@RoleID", updateRoleReqParams.RoleID),
                new SqlParameter ("@UserInfo", updateRoleReqParams.UserInfo),
                new SqlParameter ("@Delete", updateRoleReqParams.Delete),
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Role Info

        public static async Task<DataTable> UpdateRoleInfo(UpdateRoleInfoReqParams updateRoleInfoReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@RoleID", updateRoleInfoReqParams.RoleID),
                new SqlParameter ("@UserInfo", updateRoleInfoReqParams.UserInfo),
                new SqlParameter ("@Description", updateRoleInfoReqParams.Description),
                new SqlParameter ("@Delete", updateRoleInfoReqParams.Delete),
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete App Role

        public static async Task<DataTable> DeleteRole(DeleteRoleReqParams deleteRoleReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@RoleID", deleteRoleReqParams.RoleID),
                new SqlParameter ("@Delete", deleteRoleReqParams.Delete),
            };
            return await CGD.DTWithParamAsync(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

    }
}
