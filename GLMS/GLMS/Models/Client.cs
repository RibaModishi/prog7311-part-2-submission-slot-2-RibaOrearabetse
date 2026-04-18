namespace GLMS.Models;

// techmove client row - one client can have lots of contracts over time
public class Client
{
    // pk - sql makes this auto number (identity)
    public int ClientId { get; set; }

    public string Name { get; set; } = string.Empty;

    // phone/email etc - nullable bc not everyone fills everything on demo data
    public string? ContactDetails { get; set; }

    public string? Region { get; set; }

    // list of contracts for this client - ef uses this + Contract.ClientId for the link
    // = new List<> so foreach doesnt blow up with null
    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
