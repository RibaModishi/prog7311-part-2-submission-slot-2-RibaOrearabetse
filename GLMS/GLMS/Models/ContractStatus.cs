namespace GLMS.Models;

// contract "mood" for the POE - enum saves as 0,1,2,3 in sql (easier than storing text)
public enum ContractStatus
{
    Draft = 0,     // still busy filling in details
    Active = 1,    // normal running contract
    Expired = 2,   // past end date / not valid anymore - cant create service requests on this (brief)
    OnHold = 3     // paused - also blocks new service requests (brief)
}
