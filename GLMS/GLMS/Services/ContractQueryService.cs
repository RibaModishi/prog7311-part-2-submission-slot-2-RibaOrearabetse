using GLMS.Data;
using GLMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Services
{
    public class ContractQueryService
    {
        private readonly GlmsDbContext _db;

        public ContractQueryService(GlmsDbContext db)
        {
            _db = db;
        }

        /// <param name="status">
        /// When set, only contracts with this <see cref="ContractStatus"/> are returned.
        /// When null, status is not filtered (all statuses allowed by other filters).
        /// </param>
        /// <param name="rangeStart">
        /// Optional start of the admin date range. When null with no end date, the query does not
        /// apply a lower bound from this parameter (see your method body for combined rules).
        /// </param>
        /// <param name="rangeEnd">
        /// Optional end of the admin date range. When null with no start date, the query does not
        /// apply an upper bound from this parameter.
        /// </param>
        public IQueryable<Contract> GetContractForAdmin(
            ContractStatus? status,
            DateTime? rangeStart,
            DateTime? rangeEnd
            )
        {
            IQueryable<Contract> q = _db.Contracts
                .Include(c => c.Client);

            if (status.HasValue)
            q = q.Where(c => c.Status == status.Value);

            if (rangeStart.HasValue && rangeEnd.HasValue)
            {
                var start = rangeStart.Value.Date;
                var end = rangeEnd.Value.Date;

                //Overlap: contract active at any point in the range
                q = q.Where(c => c.StartDate.Date <= end && c.EndDate.Date >= start);

                //If brief meant "Contracts whose StartDate falls in the range" instead of "Contracts active at any point in the range", use this:
                // q = q.Where(c => c.StartDate.Date >= start && c.StartDate.Date <= end);
            }
            else if (rangeStart.HasValue)
            {
                var start = rangeStart.Value.Date;
                q = q.Where(c=> c.EndDate.Date >= start);
            }
            else if (rangeEnd.HasValue)
            {
                var end = rangeEnd.Value.Date;
                q = q.Where(c => c.StartDate.Date <= end);
            }
            return q.OrderByDescending(c => c.StartDate);
        }
    }
}

// References (accessed 20 Apr 2026):
// - LINQ basic operations: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/basic-linq-query-operations
// - EF Core querying data: https://learn.microsoft.com/en-us/ef/core/querying/
// - EF Core related data Include(): https://learn.microsoft.com/en-us/ef/core/querying/related-data/
