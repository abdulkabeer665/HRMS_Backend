using Microsoft.Data.SqlClient;
using System.Data;

namespace HRMS_Backend.DAL
{
    public class DbReports
    {

        #region Declaration

        Connection con;
        DataTable dt;
        DataSet ds;
        SqlDataAdapter da;
        SqlCommand cmd;
        string query = string.Empty;

        #endregion

        #region With Parameters

        public DataTable DTWithParam(string storedProcedure, SqlParameter[] parameters, int connect)
        {
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                cmd = new SqlCommand();

                con = new Connection();
                using (sqlcon = con.GetDataBaseConnection())
                {
                    query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    int cmdTimeout = Convert.ToInt32(ConfigSettings.ConfigSettings_id(4));
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i]);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                DataTable dt2 = new DataTable();
                DataRow dtRow = dt2.NewRow();
                dt2.Columns.Add("ErrorMessage");
                dtRow["ErrorMessage"] = errorMessage;
                dt2.Rows.Add(dtRow);
                return dt2;
            }
            finally
            {
                if (sqlcon != null)
                {
                    sqlcon.Close();
                    sqlcon.Dispose();
                }
            }
        }

        public async Task<DataTable> DTWithParamAsync(string storedProcedure, SqlParameter[] parameters, int connect)
        {
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                SqlCommand cmd = new SqlCommand();
                Connection con = new Connection();

                // Open the database connection asynchronously
                using (sqlcon = con.GetDataBaseConnection())
                {
                    string query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);

                    int cmdTimeout = Convert.ToInt32(ConfigSettings.ConfigSettings_id(4)); // Configurable timeout

                    // Add parameters to the command asynchronously
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }

                    cmd.CommandType = CommandType.StoredProcedure;

                    DataTable dt = new DataTable();

                    // Asynchronously execute the reader and load the result into DataTable
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        dt.Load(reader);
                    }

                    return dt;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a DataTable with the error message
                string errorMessage = ex.Message;
                DataTable dt2 = new DataTable();
                dt2.Columns.Add("ErrorMessage");
                DataRow dtRow = dt2.NewRow();
                dtRow["ErrorMessage"] = errorMessage;
                dt2.Rows.Add(dtRow);
                return dt2;
            }
            finally
            {
                // Ensure the connection is properly closed and disposed
                sqlcon?.Close();
                sqlcon?.Dispose();
            }
        }


        public DataTable DTWithParamSecondDB(string storedProcedure, SqlParameter[] parameters, int connect)
        {
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                cmd = new SqlCommand();

                con = new Connection(connect);

                using (sqlcon = con.GetDataBaseConnection())
                {
                    query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    int cmdTimeout = Convert.ToInt32(ConfigSettings.ConfigSettings_id(4));
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i]);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                DataTable dt2 = new DataTable();
                DataRow dtRow = dt2.NewRow();
                dt2.Columns.Add("ErrorMessage");
                dtRow["ErrorMessage"] = errorMessage;
                dt2.Rows.Add(dtRow);
                return dt2;
            }
            finally
            {
                if (sqlcon != null)
                {
                    sqlcon.Close();
                    sqlcon.Dispose();
                }
            }
        }

        #endregion

        #region Without Parameters

        public DataTable DTWithOutParam(string storedProcedure, int connect)
        {
            try
            {

                cmd = new SqlCommand();

                con = new Connection();
                using (SqlConnection sqlcon = con.GetDataBaseConnection())
                {
                    query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    int cmdTimeout = Convert.ToInt32(ConfigSettings.ConfigSettings_id(4));
                    cmd.CommandTimeout = cmdTimeout;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                DataTable dt2 = new DataTable();
                DataRow dtRow = dt2.NewRow();
                dt2.Columns.Add("ErrorMessage");
                dtRow["ErrorMessage"] = errorMessage;
                dt2.Rows.Add(dtRow);
                return dt2;
            }
        }

        #endregion

        #region DataSet With Parameters

        public async Task<DataSet> DSWithParamAsync(string storedProcedure, SqlParameter[] parameters, int connect)
        {
            try
            {
                cmd = new SqlCommand();

                con = new Connection();
                using (SqlConnection sqlcon = con.GetDataBaseConnection())
                {
                    string query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    var configurationBuilder = new ConfigurationBuilder();

                    // Fetch timeout from configuration
                    int cmdTimeout = Convert.ToInt32(ConfigSettings.ConfigSettings_id(4));
                    cmd.CommandTimeout = cmdTimeout;

                    // Add parameters to the command
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i]);
                    }

                    cmd.CommandType = CommandType.StoredProcedure;

                    // Create a new SqlDataAdapter for async operation
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    DataSet ds = new DataSet();

                    // Use FillAsync to asynchronously fill the DataSet
                    await Task.Run(() => da.Fill(ds));

                    return ds;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a DataSet with the error message
                string errorMessage = ex.Message;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dtRow = dt.NewRow();
                dt.Columns.Add("ErrorMessage");
                dtRow["ErrorMessage"] = errorMessage;
                dt.Rows.Add(dtRow);
                ds.Tables.Add(dt);
                return ds;
            }
        }



        public DataSet DSWithParam(string storedProcedure, SqlParameter[] parameters, int connect)
        {
            try
            {
                cmd = new SqlCommand();

                con = new Connection();
                using (SqlConnection sqlcon = con.GetDataBaseConnection())
                {
                    query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    var configurationBuilder = new ConfigurationBuilder();
                    int cmdTimeout = Convert.ToInt32(ConfigSettings.ConfigSettings_id(4));
                    cmd.CommandTimeout = cmdTimeout;
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i]);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dtRow = dt.NewRow();
                dt.Columns.Add("ErrorMessage");
                dtRow["ErrorMessage"] = errorMessage;
                dt.Rows.Add(dtRow);
                ds.Tables.Add(dt);
                return ds;
            }
        }

        public DataSet DSWithParamSecondDB(string storedProcedure, SqlParameter[] parameters, int connect)
        {
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                cmd = new SqlCommand();

                con = new Connection(connect);

                using (sqlcon = con.GetDataBaseConnection())
                {
                    query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    int cmdTimeout = Convert.ToInt32(ConfigSettings.ConfigSettings_id(4));
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i]);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dtRow = dt.NewRow();
                dt.Columns.Add("ErrorMessage");
                dtRow["ErrorMessage"] = errorMessage;
                dt.Rows.Add(dtRow);
                ds.Tables.Add(dt);
                return ds;
            }
            finally
            {
                if (sqlcon != null)
                {
                    sqlcon.Close();
                    sqlcon.Dispose();
                }
            }
        }

        #endregion

        #region DataSet WithOut Parameters

        public DataSet DSWithOutParam(string storedProcedure, int connect)
        {
            try
            {
                cmd = new SqlCommand();

                con = new Connection();
                using (SqlConnection sqlcon = con.GetDataBaseConnection())
                {
                    query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    var configurationBuilder = new ConfigurationBuilder();
                    int cmdTimeout = Convert.ToInt32(ConfigSettings.ConfigSettings_id(4));
                    cmd.CommandTimeout = cmdTimeout;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dtRow = dt.NewRow();
                dt.Columns.Add("ErrorMessage");
                dtRow["ErrorMessage"] = errorMessage;
                dt.Rows.Add(dtRow);
                ds.Tables.Add(dt);
                return ds;
            }
        }

        #endregion

    }
}
