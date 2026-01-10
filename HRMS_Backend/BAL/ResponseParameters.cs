namespace HRMS_Backend.BAL
{
    public class ResponseParameters
    {

        #region ResponseMsg

        public class Message
        {
            public string message { get; set; }
            public string status { get; set; }
        }

        #endregion

        #region LoginResponse

        public class LoginRes
        {
            public string LoginName { get; set; }
            public string EmpID { get; set; }
            public string UserName { get; set; }
            public string RoleID { get; set; }
            public string Role { get; set; }
            public string Designation { get; set; }
            public string Department { get; set; }
            public string Status { get; set; }
            public string Message { get; set; }
            public string? ValidTill { get; set; }
            public string Token { get; set; }
            public string? RefreshToken { get; set; }
            public string RefreshTokenNew { get; set; }

        }

        #endregion

    }
}
