using System.ComponentModel.DataAnnotations;
using GLMS.Models;

namespace GLMS.ViewModels;

// External API section (USD→ZAR on save): assisted by DeepSeek AI — i reviewed.
public class CreateServiceRequestViewModel
{
    [Display(Name = "Contract")]
    [Required]
    public int ContractId { get; set; }

    [Required]
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    // Posted USD; server converts to ZAR into ServiceRequest.Cost
    [Display(Name = "Cost (USD)")]
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Enter a USD amount greater than 0.")]
    public decimal CostUsd { get; set; }

    public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.Submitted;
}
