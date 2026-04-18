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
            IQueryable<Contract> q = 
        }
    }
}
