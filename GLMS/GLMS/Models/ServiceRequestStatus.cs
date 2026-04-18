namespace GLMS.Models;

// separate from contract status - this is just for tracking the actual job/request
// (assignment didnt give exact names here so i picked basic workflow ones)
public enum ServiceRequestStatus
{
    Draft = 0,
    Submitted = 1,
    InProgress = 2,
    Completed = 3,
    Cancelled = 4
}
