using HRMS_Backend.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;
using static HRMS_Backend.BAL.RequestParameters;
using static HRMS_Backend.BAL.ResponseParameters;

namespace HRMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        #region "Declaration"

        private readonly string _fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "SickLeavesAttachments");
        private readonly string SP_DeleteFileParamsAgainstRefNo = "[dbo].[SP_DeleteFileParamsAgainstRefNo]";

        #endregion

        #region "Download File"

        [HttpPost("DownloadFile")]
        [Authorize]
        public async Task<IActionResult> DownloadFile([FromBody] FileReqParam file)
        {
            // Sanitize the filename to prevent path traversal attacks
            var sanitizedFileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(_fileDirectory, sanitizedFileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found");
            }

            // Get MIME type (optional, default to application/octet-stream)
            var mimeType = "application/octet-stream";

            // Return the file
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, mimeType, sanitizedFileName);
        }

        #endregion

        #region "Delete File"

        [HttpDelete("DeleteFile")]
        [Authorize]
        public async Task<IActionResult> DeleteFile([FromBody] FileReqParam file)
        {
            Message msg = new Message();
            var sanitizedFileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(_fileDirectory, sanitizedFileName);
            if (!System.IO.File.Exists(filePath))
            {
                msg.message = "File not found.";
                msg.status = "401";
                return Ok(msg);
            };
            string fileName = Path.GetFileName(file.FileName);
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string prefix = nameWithoutExtension.Split('_')[0];
            DataTable dt = await DataLogic.DeleteFileFromDB(prefix, SP_DeleteFileParamsAgainstRefNo);
            System.IO.File.Delete(filePath);
            msg.message = "File deleted successfully.";
            msg.status = "200";
            return Ok(msg);
        }

        #endregion

    }
}
