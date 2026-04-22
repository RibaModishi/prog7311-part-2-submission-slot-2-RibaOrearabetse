using GLMS.Data;
using GLMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Services
{
    public class ServiceRequestWorkflowService
    {

        private readonly GlmsDbContext _db;

        public ServiceRequestWorkflowService(GlmsDbContext db)
        {
            _db = db;
        }

        // Brief Rule: cannot create a service request if the parent contract is expired or on hold - this is a business rule so i put it in the service layer instead of the controller
        public async Task EnsureCanCreateServiceRequestAsync(int contractId, CancellationToken ct = default)
        {
            var status = await _db.Contracts
                .Where(c => c.ContractId == contractId)
                .Select(c => c.Status)
                .FirstOrDefaultAsync(ct);

            if (status == default && !await _db.Contracts.AnyAsync(c => c.ContractId == contractId, ct))
            {
                throw new InvalidOperationException
                    ($"Contract with ID {contractId} does not exist.");
            }
            if (status == ContractStatus.Expired || status == ContractStatus.OnHold)
            {
                throw new InvalidOperationException
                    ($"Cannot create a service request for contract with ID {contractId} because its status is {status}.");
            }
        }
    }
}

// References (accessed 20 Apr 2026):
// - EF Core querying data: https://learn.microsoft.com/en-us/ef/core/querying/
// - ASP.NET Core controllers and actions: https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/actions
