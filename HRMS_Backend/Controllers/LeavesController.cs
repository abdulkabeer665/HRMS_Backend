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
    public class LeavesController : ControllerBase
    {

        #region "Declaration"
        
        public readonly static string SP_GetLeaveAvailedAgainstLeaveType = "[dbo].[SP_GetLeaveAvailedAgainstLeaveType]";
        public readonly static string SP_InsertUpdateDeleteCasualLeave = "[dbo].[SP_InsertUpdateDeleteCasualLeave]";
        public readonly static string SP_GetPendingLeaves = "[dbo].[SP_GetPendingLeaves]";
        public readonly static string SP_GetLeaveInfoAgainstRefNo = "[dbo].[SP_GetLeaveInfoAgainstRefNo]";
        public readonly static string SP_GetEmpIDAgainstDesignation = "[dbo].[SP_GetEmpIDAgainstDesignation]";
        public readonly static string SP_CheckLeaveDaysRemaining = "[dbo].[SP_CheckLeaveDaysRemaining]";
        public readonly static string SP_CheckLeaveDatesAlreadyOccupied = "[dbo].[SP_CheckLeaveDatesAlreadyOccupied]";
        public readonly static string SP_GetLeaveDatesAlreadyOccupied = "[dbo].[SP_GetLeaveDatesAlreadyOccupied]";

        #endregion

        #region Get Leave Availed Against Leave Type

        /// <summary>
        /// Get Leaves availed against leave type and emp id
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost("GetLeaveAvailedAgainstLeaveType")]
        [Authorize]
        public async Task<IActionResult> GetLeaveAvailedAgainstLeaveType([FromBody] LeaveReqParams leaveReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = await DataLogic.GetLeaveAvailedAgainstLeaveType(leaveReqParams, SP_GetLeaveAvailedAgainstLeaveType);
                dt.Columns.Add("EncryptedRefNo", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["EncryptedRefNo"] = EncryptDecryptPassword.EncryptQueryString(dt.Rows[i]["RefNo"].ToString());
                }
                return Ok(dt);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion Get Leave Availed Against Leave Type

        #region Pending Leaves

        /// <summary>
        /// Get Leaves availed against leave type and emp id
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost("PendingLeaves")]
        [Authorize]
        public async Task<IActionResult> PendingLeaves([FromBody] PendingLeavesReqParams pendingLeavesReqParams)
        {
            Message msg = new Message();
            //EncryptDecryptPassword EDP = new EncryptDecryptPassword();
            try
            {
                DataTable dt = await DataLogic.PendingLeaves(pendingLeavesReqParams, SP_GetPendingLeaves);
                dt.Columns.Add("EncryptedRefNo", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["EncryptedRefNo"] = EncryptDecryptPassword.EncryptQueryString(dt.Rows[i]["RefNo"].ToString());
                }
                return Ok(dt);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion Get Leave Availed Against Leave Type

        #region Insert Casual Leave

        /// <summary>
        /// Insert Casual Leave
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost("InsertCasualLeave")]
        [Authorize]
        public async Task<IActionResult> InsertCasualLeave([FromBody] CasualLeaveReqParams casualLeaveReqParams)
        {
            Message msg = new Message();
            try
            {
                int totalLeaveAllowed = 7; // Total leave entitlement
                int leaveDaysAvailed = 0;
                int leaveExistsCount = 0;

                // Get already availed leave days
                var leaveDaysAvailedDT = await DataLogic.CheckLeaveDaysRemaining(casualLeaveReqParams, SP_CheckLeaveDaysRemaining);

                if (leaveDaysAvailedDT != null && leaveDaysAvailedDT.Rows.Count > 0)
                {
                    leaveDaysAvailed = Convert.ToInt32(leaveDaysAvailedDT.Rows[0]["DateDifference"]);
                }

                // Calculate applied leave days
                int appliedLeaveDays = (Convert.ToDateTime(casualLeaveReqParams.ToDate) - Convert.ToDateTime(casualLeaveReqParams.FromDate)).Days + 1;

                // Remaining leave days
                int remainingLeaves = totalLeaveAllowed - leaveDaysAvailed;

                // 1️⃣ Already availed all leaves
                if (leaveDaysAvailed >= totalLeaveAllowed)
                {
                    msg.message = "You've already availed all your leaves.";
                    msg.status = "401";
                    return Ok(msg);
                }

                // 2️⃣ Applied leave days exceed remaining leave balance
                if (appliedLeaveDays > remainingLeaves)
                {
                    msg.message = $"Applied leave days exceed your available leave balance. Remaining days are {remainingLeaves}.";
                    msg.status = "401";
                    return Ok(msg);
                }

                // 3️⃣ Check if selected leave dates overlap existing leaves
                var leaveExistsCountDT = await DataLogic.CheckLeaveDatesExists(casualLeaveReqParams, SP_CheckLeaveDatesAlreadyOccupied);

                if (leaveExistsCountDT != null && leaveExistsCountDT.Rows.Count > 0)
                {
                    leaveExistsCount = Convert.ToInt32(leaveExistsCountDT.Rows[0]["LeaveExistsCount"]);
                }

                if (leaveExistsCount > 0)
                {
                    msg.message = "Selected leave dates are already occupied.";
                    msg.status = "401";
                    return Ok(msg);
                }
                else
                {
                    DataTable dt = await DataLogic.InsertCasualLeave(casualLeaveReqParams, SP_InsertUpdateDeleteCasualLeave);
                    msg.message = dt.Rows[0]["Message"].ToString();
                    msg.status = dt.Rows[0]["Status"].ToString();
                    return Ok(msg);
                };                
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            };
        }

        #endregion Dashboard Menus

        #region Update Casual Leave

        /// <summary>
        /// Update Casual Leave
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost("UpdateCasualLeave")]
        [Authorize]
        public async Task<IActionResult> UpdateCasualLeave([FromBody] CasualLeaveReqParams casualLeaveReqParams)
        {
            Message msg = new Message();
            try
            {
                if (casualLeaveReqParams.FromDate != "" || casualLeaveReqParams.ToDate != "")
                {

                    int totalLeaveAllowed = 7; // Total leave entitlement
                    int leaveDaysAvailed = 0;
                    int leaveExistsCount = 0;

                    // Get already availed leave days
                    var leaveDaysAvailedDT = await DataLogic.CheckLeaveDaysRemaining(casualLeaveReqParams, SP_CheckLeaveDaysRemaining);

                    if (leaveDaysAvailedDT != null && leaveDaysAvailedDT.Rows.Count > 0)
                    {
                        leaveDaysAvailed = Convert.ToInt32(leaveDaysAvailedDT.Rows[0]["DateDifference"]);
                    }

                    // Calculate applied leave days
                    int appliedLeaveDays = (Convert.ToDateTime(casualLeaveReqParams.ToDate) - Convert.ToDateTime(casualLeaveReqParams.FromDate)).Days + 1;

                    // Remaining leave days
                    int remainingLeaves = totalLeaveAllowed - leaveDaysAvailed;

                    // 1️⃣ Already availed all leaves
                    if (leaveDaysAvailed >= totalLeaveAllowed)
                    {
                        msg.message = "You've already availed all your leaves.";
                        msg.status = "401";
                        return Ok(msg);
                    }

                    // 2️⃣ Applied leave days exceed remaining leave balance
                    if (appliedLeaveDays > remainingLeaves)
                    {
                        msg.message = $"Applied leave days exceed your available leave balance. Remaining days are {remainingLeaves}.";
                        msg.status = "401";
                        return Ok(msg);
                    }

                    // 3️⃣ Check if selected leave dates overlap existing leaves
                    var leaveExistsCountDT = await DataLogic.CheckLeaveDatesExists(casualLeaveReqParams, SP_CheckLeaveDatesAlreadyOccupied);

                    if (leaveExistsCountDT != null && leaveExistsCountDT.Rows.Count > 0)
                    {
                        leaveExistsCount = Convert.ToInt32(leaveExistsCountDT.Rows[0]["LeaveExistsCount"]);
                    }

                    if (leaveExistsCount > 0)
                    {
                        msg.message = "Selected leave dates are already occupied.";
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {

                        string empIDAgainstDesignation = string.Empty;
                        if (casualLeaveReqParams.ManagerApproved == "Approved")
                        {
                            DataTable dt2 = await DataLogic.GetEmpIDAgainstDesignation("HR Head", SP_GetEmpIDAgainstDesignation);
                            empIDAgainstDesignation = dt2.Rows[0]["EmpID"].ToString();
                            casualLeaveReqParams.HRID = empIDAgainstDesignation;
                        }
                        DataTable dt = await DataLogic.UpdateCasualLeave(casualLeaveReqParams, SP_InsertUpdateDeleteCasualLeave);
                        msg.message = dt.Rows[0]["Message"].ToString();
                        msg.status = dt.Rows[0]["Status"].ToString();
                        return Ok(msg);

                    }

                }
                else
                {

                    string empIDAgainstDesignation = string.Empty;
                    if (casualLeaveReqParams.ManagerApproved == "Approved")
                    {
                        DataTable dt2 = await DataLogic.GetEmpIDAgainstDesignation("HR Head", SP_GetEmpIDAgainstDesignation);
                        empIDAgainstDesignation = dt2.Rows[0]["EmpID"].ToString();
                        casualLeaveReqParams.HRID = empIDAgainstDesignation;
                    }
                    DataTable dt = await DataLogic.UpdateCasualLeave(casualLeaveReqParams, SP_InsertUpdateDeleteCasualLeave);
                    msg.message = dt.Rows[0]["Message"].ToString();
                    msg.status = dt.Rows[0]["Status"].ToString();
                    return Ok(msg);

                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion Dashboard Menus

        #region Get Leave Info Against Ref No

        /// <summary>
        /// Get Leave Info Against Ref No and Leave Type for SickCasual and for Annual
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost("GetLeaveInfoAgainstRefNo")]
        [Authorize]
        public async Task<IActionResult> GetLeaveInfoAgainstRefNo([FromBody] LeaveInfoReqParams leaveInfoReqParams)
        {
            Message msg = new Message();
            try
            {
                if (leaveInfoReqParams.RefNo == "")
                {
                    msg.message = "Ref no is not provided please select leave again.";
                    msg.status = "404";
                    return Ok(msg);
                }
                else
                {
                    leaveInfoReqParams.RefNo = EncryptDecryptPassword.DecryptQueryString(leaveInfoReqParams.RefNo);
                    DataTable dt = await DataLogic.GetLeaveInfoAgainstRefNo(leaveInfoReqParams, SP_GetLeaveInfoAgainstRefNo);
                    return Ok(dt);
                }
                
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion Dashboard Menus

        #region Insert Sick Leave

        /// <summary>
        /// Insert Sick Leave
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost("InsertSickLeave")]
        [Authorize]
        public async Task<IActionResult> InsertSickLeave([FromForm] SickLeaveReqParams sickLeaveReqParams)
        {
            Message msg = new Message();
            CasualLeaveReqParams CLRP = new CasualLeaveReqParams();
            try
            {
                int totalLeaveAllowed = 7; // Total leave entitlement
                int leaveDaysAvailed = 0;
                int leaveExistsCount = 0;

                if (sickLeaveReqParams.myFile == null || sickLeaveReqParams.myFile.Length == 0)
                {
                    msg.message = "Attachment is required for Sick Leave";
                    msg.status = "401";
                    return Ok(msg);
                }

                // Get already availed leave days
                CLRP.EmpID = sickLeaveReqParams.EmpID;
                var leaveDaysAvailedDT = await DataLogic.CheckLeaveDaysRemaining(CLRP, SP_CheckLeaveDaysRemaining);

                if (leaveDaysAvailedDT != null && leaveDaysAvailedDT.Rows.Count > 0)
                {
                    leaveDaysAvailed = Convert.ToInt32(leaveDaysAvailedDT.Rows[0]["DateDifference"]);
                }

                // Calculate applied leave days
                int appliedLeaveDays = (Convert.ToDateTime(sickLeaveReqParams.ToDate) - Convert.ToDateTime(sickLeaveReqParams.FromDate)).Days + 1;

                // Remaining leave days
                int remainingLeaves = totalLeaveAllowed - leaveDaysAvailed;

                // 1️⃣ Already availed all leaves
                if (leaveDaysAvailed >= totalLeaveAllowed)
                {
                    msg.message = "You've already availed all your leaves.";
                    msg.status = "401";
                    return Ok(msg);
                }

                // 2️⃣ Applied leave days exceed remaining leave balance
                if (appliedLeaveDays > remainingLeaves)
                {
                    msg.message = $"Applied leave days exceed your available leave balance. Remaining days are {remainingLeaves}.";
                    msg.status = "401";
                    return Ok(msg);
                }

                // 3️⃣ Check if selected leave dates overlap existing leaves
                CLRP.EmpID = sickLeaveReqParams.EmpID;
                CLRP.FromDate = sickLeaveReqParams.FromDate;
                CLRP.ToDate = sickLeaveReqParams.ToDate;
                var leaveExistsCountDT = await DataLogic.CheckLeaveDatesExists(CLRP, SP_CheckLeaveDatesAlreadyOccupied);

                if (leaveExistsCountDT != null && leaveExistsCountDT.Rows.Count > 0)
                {
                    leaveExistsCount = Convert.ToInt32(leaveExistsCountDT.Rows[0]["LeaveExistsCount"]);
                }

                if (leaveExistsCount > 0)
                {
                    msg.message = "Selected leave dates are already occupied.";
                    msg.status = "401";
                    return Ok(msg);
                }
                else
                {

                    var sickLeaveUploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "SickLeavesAttachments");

                    if (!Directory.Exists(sickLeaveUploadDirectory))
                    {
                        Directory.CreateDirectory(sickLeaveUploadDirectory);
                    }

                    var fileName = Path.GetFileName(sickLeaveReqParams.myFile.FileName);
                    var filePath =  sickLeaveUploadDirectory;
                    //var filePath =  Path.Combine(sickLeaveUploadDirectory, fileName);
                    var fileExtension = Path.GetExtension(fileName).ToLower();

                    DataTable dt = await DataLogic.InsertSickLeave(sickLeaveReqParams, fileName, filePath, fileExtension, SP_InsertUpdateDeleteCasualLeave);
                    msg.message = dt.Rows[0]["Message"].ToString();
                    msg.status = dt.Rows[0]["Status"].ToString();
                    string currentRefNo = dt.Rows[0]["currentRefNo"].ToString();

                    if (msg.status == "200")
                    {
                        string finalFileName = currentRefNo + "_" + fileName;   //RefNo_ABC.png
                        string finalFilePath = Path.Combine(sickLeaveUploadDirectory, finalFileName);
                        if (System.IO.File.Exists(finalFilePath))
                        {
                            System.IO.File.Delete(finalFilePath);
                        }

                        using (var stream = new FileStream(finalFilePath, FileMode.Create))
                        {
                            await sickLeaveReqParams.myFile.CopyToAsync(stream);
                        }
                    }
                    return Ok(msg);
                };
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }
            ;
        }

        #endregion Dashboard Menus

        #region Update Sick Leave

        /// <summary>
        /// Update Sick Leave
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost("UpdateSickLeave")]
        [Authorize]
        public async Task<IActionResult> UpdateSickLeave([FromForm] SickLeaveReqParams sickLeaveReqParams)
        {
            Message msg = new Message();
            CasualLeaveReqParams CLRP = new CasualLeaveReqParams();
            try
            {
                if ((sickLeaveReqParams.FromDate != "" && sickLeaveReqParams.FromDate != null) || (sickLeaveReqParams.ToDate != "" && sickLeaveReqParams.ToDate != null) )
                {

                    int totalLeaveAllowed = 7; // Total leave entitlement
                    int leaveDaysAvailed = 0;
                    int leaveExistsCount = 0;

                    // Get already availed leave days
                    CLRP.EmpID = sickLeaveReqParams.EmpID;
                    var leaveDaysAvailedDT = await DataLogic.CheckLeaveDaysRemaining(CLRP, SP_CheckLeaveDaysRemaining);

                    if (leaveDaysAvailedDT != null && leaveDaysAvailedDT.Rows.Count > 0)
                    {
                        leaveDaysAvailed = Convert.ToInt32(leaveDaysAvailedDT.Rows[0]["DateDifference"]);
                    }

                    // Calculate applied leave days
                    int appliedLeaveDays = (Convert.ToDateTime(sickLeaveReqParams.ToDate) - Convert.ToDateTime(sickLeaveReqParams.FromDate)).Days + 1;

                    // Remaining leave days
                    int remainingLeaves = totalLeaveAllowed - leaveDaysAvailed;

                    // 1️⃣ Already availed all leaves
                    if (leaveDaysAvailed >= totalLeaveAllowed)
                    {
                        msg.message = "You've already availed all your leaves.";
                        msg.status = "401";
                        return Ok(msg);
                    }

                    // 2️⃣ Applied leave days exceed remaining leave balance
                    if (appliedLeaveDays > remainingLeaves)
                    {
                        msg.message = $"Applied leave days exceed your available leave balance. Remaining days are {remainingLeaves}.";
                        msg.status = "401";
                        return Ok(msg);
                    }

                    // 3️⃣ Check if selected leave dates overlap existing leaves
                    CLRP.FromDate = sickLeaveReqParams.FromDate;
                    CLRP.ToDate = sickLeaveReqParams.ToDate;
                    CLRP.EmpID = sickLeaveReqParams.EmpID;
                    var leaveExistsCountDT = await DataLogic.CheckLeaveDatesExists(CLRP, SP_CheckLeaveDatesAlreadyOccupied);

                    if (leaveExistsCountDT != null && leaveExistsCountDT.Rows.Count > 0)
                    {
                        leaveExistsCount = Convert.ToInt32(leaveExistsCountDT.Rows[0]["LeaveExistsCount"]);
                    }

                    if (leaveExistsCount > 0)
                    {
                        msg.message = "Selected leave dates are already occupied.";
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {

                        string empIDAgainstDesignation = string.Empty;
                        if (sickLeaveReqParams.ManagerApproved == "Approved")
                        {
                            DataTable dt2 = await DataLogic.GetEmpIDAgainstDesignation("HR Head", SP_GetEmpIDAgainstDesignation);
                            empIDAgainstDesignation = dt2.Rows[0]["EmpID"].ToString();
                            sickLeaveReqParams.HRID = empIDAgainstDesignation;
                        }
                        string sickLeaveUploadDirectory = string.Empty;

                        if (sickLeaveReqParams.myFile != null) //Contains Attachment
                        {
                            sickLeaveUploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "SickLeavesAttachments");
                            var fileName = Path.GetFileName(sickLeaveReqParams.myFile.FileName);
                            var filePath = sickLeaveUploadDirectory;
                            var fileExtension = Path.GetExtension(fileName).ToLower();

                            DataTable dt = await DataLogic.UpdateSickLeaveWithAttachment(sickLeaveReqParams, fileName, filePath, fileExtension, SP_InsertUpdateDeleteCasualLeave);
                            msg.message = dt.Rows[0]["Message"].ToString();
                            msg.status = dt.Rows[0]["Status"].ToString();
                            string currentRefNo = dt.Rows[0]["currentRefNo"].ToString();

                            if (msg.status == "200")
                            {
                                string finalFileName = currentRefNo + "_" + fileName;   //RefNo_ABC.png
                                string finalFilePath = Path.Combine(sickLeaveUploadDirectory, finalFileName);
                                if (System.IO.File.Exists(finalFilePath))
                                {
                                    System.IO.File.Delete(finalFilePath);
                                }

                                using (var stream = new FileStream(finalFilePath, FileMode.Create))
                                {
                                    await sickLeaveReqParams.myFile.CopyToAsync(stream);
                                }
                            }

                        }
                        else
                        {
                            DataTable dt2 = await DataLogic.UpdateSickLeaveWithoutAttachment(sickLeaveReqParams, SP_InsertUpdateDeleteCasualLeave);
                            msg.message = dt2.Rows[0]["Message"].ToString();
                            msg.status = dt2.Rows[0]["Status"].ToString();
                        }
                    }
                }
                else
                {
                    string empIDAgainstDesignation = string.Empty;
                    {
                        DataTable dt2 = await DataLogic.GetEmpIDAgainstDesignation("HR Head", SP_GetEmpIDAgainstDesignation);
                        empIDAgainstDesignation = dt2.Rows[0]["EmpID"].ToString();
                        sickLeaveReqParams.HRID = empIDAgainstDesignation;
                    }

                    string sickLeaveUploadDirectory = string.Empty;
                    
                    if (sickLeaveReqParams.myFile != null) //Contains Attachment
                    {
                        sickLeaveUploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "SickLeavesAttachments");
                        var fileName = Path.GetFileName(sickLeaveReqParams.myFile.FileName);
                        var filePath = sickLeaveUploadDirectory;
                        var fileExtension = Path.GetExtension(fileName).ToLower();

                        DataTable dt = await DataLogic.UpdateSickLeaveWithAttachment(sickLeaveReqParams, fileName, filePath, fileExtension, SP_InsertUpdateDeleteCasualLeave);
                        msg.message = dt.Rows[0]["Message"].ToString();
                        msg.status = dt.Rows[0]["Status"].ToString();
                        string currentRefNo = dt.Rows[0]["currentRefNo"].ToString();

                        if (msg.status == "200")
                        {
                            string finalFileName = currentRefNo + "_" + fileName;   //RefNo_ABC.png
                            string finalFilePath = Path.Combine(sickLeaveUploadDirectory, finalFileName);
                            if (System.IO.File.Exists(finalFilePath))
                            {
                                System.IO.File.Delete(finalFilePath);
                            }

                            using (var stream = new FileStream(finalFilePath, FileMode.Create))
                            {
                                await sickLeaveReqParams.myFile.CopyToAsync(stream);
                            }
                        }
                        
                    }
                    else
                    {
                        DataTable dt2 = await DataLogic.UpdateSickLeaveWithoutAttachment(sickLeaveReqParams, SP_InsertUpdateDeleteCasualLeave);
                        msg.message = dt2.Rows[0]["Message"].ToString();
                        msg.status = dt2.Rows[0]["Status"].ToString();
                    }
                }
                return Ok(msg);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return BadRequest(msg);
            }

        }

        #endregion Dashboard Menus

    }
}
