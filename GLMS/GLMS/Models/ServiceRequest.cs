namespace GLMS.Models;

// logistics request - always belongs to one contract
public class ServiceRequest
{
    public int ServiceRequestId { get; set; }

    public int ContractId { get; set; }
    public Contract Contract { get; set; } = null!;

    public string Description { get; set; } = string.Empty;

    // storing money as decimal in ZAR (usd conversion happens on the form later with the api)
    public decimal Cost { get; set; }

    public ServiceRequestStatus Status { get; set; }

    // TODO when i build the create page: block save if parent contract is Expired or OnHold (brief rule)
}
