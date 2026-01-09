using Microsoft.AspNetCore.Mvc;

namespace HRMS_Backend.BAL
{
    public class RequestParameters
    {
        #region Pagination Parameters
        public class PaginationParam
        {
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 0;
        }
        #endregion

        #region User Parameters

        #region Loginparam
        public class Loginparam
        {
            public string LoginName { get; set; }
            public string Password { get; set; }

        }
        #endregion

        #region GeneratePasswordTokenParameters

        public class GeneratePasswordTokenParam
        {
            public string LoginName { get; set; }

        }

        #endregion

        #region Change Password
        public class ChangePassword
        {
            public string? LoginName { get; set; }
            public string? OldPassword { get; set; }
            public string? NewPassword { get; set; }
            public string? TransactionUser { get; set; }
        }
        #endregion

        #region ForgotPassword
        public class ForgotPassword
        {
            public string LoginName { get; set; }
            public string token { get; set; }
            public string NewPassword { get; set; }
        }

        #endregion

        #region RefreshTokenRequest

        public class RefreshTokenRequest
        {
            public string JWTToken { get; set; }
            public string RefreshToken { get; set; }
        }

        #endregion

        #region User

        public class UserReqParams
        {
            public int Add { get; set; }
            public int Inactive { get; set; }
            public int Update { get; set; }
            //public int Delete { get; set; }
            //public int Searching { get; set; }
            //public int Dropdown { get; set; }
            public string LoginName { get; set; } = "";
            public string EmpID { get; set; } = "";
            public string UserName { get; set; } = "";
            public string Password { get; set; } = "";
            public bool IsDeleted { get; set; } = false;
            public string UserAccess { get; set; } = "";
            public int RoleID { get; set; } = 0;
            //public string Var { get; set; } = "";
            //public string TransactionUser { get; set; } = "";
            //public PaginationParam PaginationParam { get; set; } = new PaginationParam();

        }

        #endregion

        #endregion

        #region Dashboard Parameters 

        public class EmpIDReqParams
        {
            public int EmpID { get; set; } = 0;
        }

        public class RoleIDReqParams
        {
            public int RoleID { get; set; } = 0;
            public string ReportingManagerID { get; set; } = "";
        }

        #endregion

        #region "Leave Parameters"

        public class LeaveReqParams
        {
            public int LeaveType { get; set; } = 0;
            public string EmpID { get; set; } = "";
            public int RoleID { get; set; } = 0;
        }

        #endregion

        public class PendingLeavesReqParams
        {
            public int LeaveType { get; set; } = 0;
            public int RoleID { get; set; } = 0;
            public string ReportingManagerID { get; set; } = "";
        }

        #region Casual Leave Parameter

        public class CasualLeaveReqParams
        {
            public int LeaveID { get; set; } = 0;
            public string RefNo { get; set; } = string.Empty;
            public int LeaveType { get; set; } = 0;
            public string FromDate { get; set; } = string.Empty;
            public string ToDate { get; set; } = string.Empty;
            public string EmpID { get; set; } = string.Empty;
            public string RequestDate { get; set; } = string.Empty;
            public string Reason { get; set; } = string.Empty;
            public string ReportingManagerID { get; set; } = string.Empty;
            public string ManagerApproved { get; set; } = string.Empty;
            public string ManagerApprovedDate { get; set; } = string.Empty;
            public string ManagerRemarks { get; set; } = string.Empty;
            public string HRID { get; set; } = string.Empty;
            public string HRApproved { get; set; } = string.Empty;
            public string HRApprovedDate { get; set; } = string.Empty;
            public string HRRemarks { get; set; } = string.Empty;
            public int LeaveStatus { get; set; } = 0;
            public string PendingFrom { get; set; } = string.Empty;
        }

        #endregion

        #region Sick Leave Parameter

        public class SickLeaveReqParams
        {
            public int LeaveID { get; set; } = 0;
            public string? RefNo { get; set; } = string.Empty;
            public int LeaveType { get; set; } = 0;
            public string? FromDate { get; set; } = string.Empty;
            public string? ToDate { get; set; } = string.Empty;
            public string EmpID { get; set; } = string.Empty;
            public string RequestDate { get; set; } = string.Empty;
            public string? Reason { get; set; } = string.Empty;
            public string ReportingManagerID { get; set; } = string.Empty;
            public string ManagerApproved { get; set; } = string.Empty;
            public string ManagerApprovedDate { get; set; } = string.Empty;
            public string ManagerRemarks { get; set; } = string.Empty;
            public string HRID { get; set; } = string.Empty;
            public string HRApproved { get; set; } = string.Empty;
            public string HRApprovedDate { get; set; } = string.Empty;
            public string HRRemarks { get; set; } = string.Empty;
            public int LeaveStatus { get; set; } = 0;
            public IFormFile? myFile { get; set; }
            public bool IsFileChanged { get; set; } = false;
            public string PendingFrom { get; set; } = string.Empty;
        }

        public class FileReqParam
        {
            public string FileName { get; set; } = "";
        }

        #endregion

        #region "Get Leave Info Parameter

        public class LeaveInfoReqParams
        {
            public string LeaveType { get; set; } = string.Empty;
            public string RefNo { get; set; } = string.Empty;
        }
        #endregion

        #region "Menu"

        public class MenuReqParams
        {
            public string? RoleID { get; set; } = "";
            public string? MenuID { get; set; } = "";
        }

        #endregion

        #region "Reporting Manager"

        public class ReportingManagerReqParams
        {
            public string ReportingManagerID { get; set; } = string.Empty;
        }
        #endregion

        #region "Roles Parameters"

        public class RoleReqParams
        {
            public string? RoleID { get; set; }

        }

        public class InsertRoleReqParams
        {
            public int RoleID { get; set; } = 0;
            public string? Description { get; set; } = "";
        }

        public class UpdateRoleReqParams
        {
            public string? RoleID { get; set; } = "";
            public List<RoleAssignOptions> roleAssignOptions_list { get; set; } = new List<RoleAssignOptions> { };
            public int Delete { get; set; }
            public int UserInfo { get; set; }
        }

        public class RoleAssignOptions
        {
            public int RoleAsingmentID { get; set; }
            public string? RoleID { get; set; } = "";
            public int Value { get; set; }
        }

        public class UpdateRoleInfoReqParams
        {
            public string? RoleID { get; set; } = "";
            public string? Description { get; set; } = "";
            public int UserInfo { get; set; }
            public int Delete { get; set; }
        }

        public class DeleteRoleReqParams
        {
            public string? RoleID { get; set; } = "";
            public int Delete { get; set; }
        }

        #endregion
    }
}
