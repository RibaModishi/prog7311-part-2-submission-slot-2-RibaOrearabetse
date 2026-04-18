namespace GLMS.Models;

// agreement between techmove and a client
public class Contract
{
    public int ContractId { get; set; }

    // fk - must match a real ClientId in Clients table
    public int ClientId { get; set; }

    // navigation - lets you do contract.Client.Name (remember Include() in queries)
    public Client Client { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // see ContractStatus enum
    public ContractStatus Status { get; set; }

    // sla / package name from the brief
    public string ServiceLevel { get; set; } = string.Empty;

    // pdf upload: i only store the filename (guid.pdf) not the whole path - file sits in wwwroot folder later
    public string? SignedAgreementFileName { get; set; }

    public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
}
