using Pledge.DATAACCESS;
using Pledge.MODELS;

namespace Pledge.BUSINESSLOGIC
{
    public class EmployeePledgeBusinessLogic
    {
        private readonly EmployeePledgeDataAccess _dataAccess;

        public EmployeePledgeBusinessLogic(EmployeePledgeDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        // Get pledge details
        public async Task<SecurityPledgeDetailsDto?> GetPledgeDetailsAsync(int cid)
        {
            if (cid <= 0)
                throw new ArgumentException("Invalid CID value");

            return await _dataAccess.GetDetailsAsync(cid);
        }

        // Submit pledge response
        public async Task<bool> SubmitPledgeAsync(SecurityPledgeSubmitDto pledge)
        {
            if (pledge == null)
                throw new ArgumentNullException(nameof(pledge));

            if (pledge.InstanceId <= 0)
                throw new ArgumentException("Invalid InstanceId");

            var rowsAffected = await _dataAccess.SubmitPledgeAsync(pledge);
            return rowsAffected > 0;
        }
    }
}
