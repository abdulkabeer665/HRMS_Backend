namespace HRMS_Backend.DAL
{
    public class GenericFunctions
    {

        public static string GetFilePath()
        {
            string applicationCurrentLocation = Directory.GetCurrentDirectory();    //Application current Location
            string employeePhotoPath = Path.Combine(applicationCurrentLocation, "Uploads", "Employee", "ProfilePhoto");
            return employeePhotoPath;
        }

        public static string GetFileLocation()
        {
            string employeePhotoPath = Path.Combine("Uploads", "Employee", "ProfilePhoto");
            return employeePhotoPath;
        }

    }
}
