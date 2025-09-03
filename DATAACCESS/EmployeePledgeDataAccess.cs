using System.Data;
using System.Data.SqlClient;
using Pledge.MODELS;

namespace Pledge.DATAACCESS
{
    public class EmployeePledgeDataAccess
    {
        private readonly string _connectionString;

        public EmployeePledgeDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Fetch details from stored procedure
        public async Task<SecurityPledgeDetailsDto?> GetDetailsAsync(int cid)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SecurityPledge_GetDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CID", cid);

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows)
            {
                Console.WriteLine($"⚠️ No rows returned for CID={cid}");
                return null;
            }

            if (await reader.ReadAsync())
            {
                // 🔹 Debug: print column names & values to console
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.WriteLine($"Column[{i}] {reader.GetName(i)} = {reader[i]}");
                }

                // 🔹 Safe mapping: check if column exists before reading
                return new SecurityPledgeDetailsDto
                {
                    InstanceId = reader.HasColumn("InstanceID") ? Convert.ToInt32(reader["InstanceID"]) : 0,
                    SPName = reader.HasColumn("SPName") ? reader["SPName"].ToString() : null,
                    SPCheckText = reader.HasColumn("SPCheckText") ? reader["SPCheckText"].ToString() : null,
                    SPHtmlBody = reader.HasColumn("SPHtmlBody") ? reader["SPHtmlBody"].ToString() : null,
                    WFStatus = reader.HasColumn("WFStatus") ? reader["WFStatus"].ToString() : null,
                    IsAgree = reader.HasColumn("IsAgree") && reader["IsAgree"] != DBNull.Value
                                ? Convert.ToBoolean(reader["IsAgree"]) : (bool?)null,
                    EmpName = reader.HasColumn("EmpName") ? reader["EmpName"].ToString() : null,
                    IsUploadAttachmentAvailable = reader.HasColumn("IsUploadAttachmentAvailable") &&
                                                  reader["IsUploadAttachmentAvailable"] != DBNull.Value &&
                                                  Convert.ToBoolean(reader["IsUploadAttachmentAvailable"]),
                    FileIndex = reader.HasColumn("FileIndex") && reader["FileIndex"] != DBNull.Value
                                ? Convert.ToInt32(reader["FileIndex"]) : (int?)null,
                    IsTimerEnabled = reader.HasColumn("IsTimerEnabled") && reader["IsTimerEnabled"] != DBNull.Value &&
                                     Convert.ToBoolean(reader["IsTimerEnabled"]),
                    TimeInSecs = reader.HasColumn("TimeInSecs") && reader["TimeInSecs"] != DBNull.Value
                                ? Convert.ToInt32(reader["TimeInSecs"]) : (int?)null,
                    SPID = reader.HasColumn("SPID") && reader["SPID"] != DBNull.Value
                                ? Convert.ToInt32(reader["SPID"]) : 0
                };
            }

            return null;
        }

        // Save pledge response
        public async Task<int> SubmitPledgeAsync(SecurityPledgeSubmitDto pledge)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SecurityPledge_Submit", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@InstanceId", pledge.InstanceId);
            cmd.Parameters.AddWithValue("@IsAgree", pledge.IsAgree);

            await con.OpenAsync();
            return await cmd.ExecuteNonQueryAsync(); // returns affected rows
        }
    }

    // 🔹 Extension method to check if a column exists
    public static class SqlDataReaderExtensions
    {
        public static bool HasColumn(this SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
