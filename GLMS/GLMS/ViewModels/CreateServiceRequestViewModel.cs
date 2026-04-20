using System.ComponentModel.DataAnnotations;
using GLMS.Models;

namespace GLMS.ViewModels
{
    // view model = shape of the CREATE form only - not the full ServiceRequest entity (keeps form simple + i can add validation here)
    public class CreateServiceRequestViewModel
    {
        // which contract this request belongs to - dropdown binds here
        [Display(Name = "Contract")]
        [Required]
        public int ContractId { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        // Range needs strings for min/max in this overload - big upper number = "practical max" for money field
        [Display(Name = "Cost (ZAR)")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Cost { get; set; }

        // controller copies this onto the real ServiceRequest row when saving
        public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.Submitted;
    }
}
