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

            if (await reader.ReadAsync())
            {
                return new SecurityPledgeDetailsDto
                {
                    InstanceId = reader["InstanceID"] != DBNull.Value ? Convert.ToInt32(reader["InstanceID"]) : 0,
                    SPName = reader["SPName"]?.ToString(),
                    SPCheckText = reader["SPCheckText"]?.ToString(),
                    SPHtmlBody = reader["SPHtmlBody"]?.ToString(),
                    WFStatus = reader["WFStatus"]?.ToString(),
                    IsAgree = reader["IsAgree"] != DBNull.Value ? Convert.ToBoolean(reader["IsAgree"]) : (bool?)null,
                    EmpName = reader["EmpName"]?.ToString(),
                    IsUploadAttachmentAvailable = reader["IsUploadAttachmentAvailable"] != DBNull.Value && Convert.ToBoolean(reader["IsUploadAttachmentAvailable"]),
                    FileIndex = reader["FileIndex"] != DBNull.Value ? Convert.ToInt32(reader["FileIndex"]) : (int?)null,
                    IsTimerEnabled = reader["IsTimerEnabled"] != DBNull.Value && Convert.ToBoolean(reader["IsTimerEnabled"]),
                    TimeInSecs = reader["TimeInSecs"] != DBNull.Value ? Convert.ToInt32(reader["TimeInSecs"]) : (int?)null,
                    SPID = reader["SPID"] != DBNull.Value ? Convert.ToInt32(reader["SPID"]) : 0
                };
            }

            return null;
        }

        // Example: Save pledge response
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
}

