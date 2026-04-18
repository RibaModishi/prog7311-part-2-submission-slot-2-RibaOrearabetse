namespace GLMS.Models;

// Lifecycle state of a client agreement. Persisted as int (0–3) in SQL via Entity Framework.
public enum ContractStatus
{
    Draft = 0,     // still busy filling in details
    Active = 1,    // normal running contract
    Expired = 2,   // past end date / not valid anymore - cant create service requests on this (brief)
    OnHold = 3     // paused - also blocks new service requests (brief)
}
