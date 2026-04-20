using GLMS.Data;
using GLMS.Models;
using GLMS.Services;
using GLMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Controllers
{
    // i split this from HomeController - anything about "logistics requests" lives here
    // [Authorize] = must be logged in (negotiate/windows auth in my case) or i get bounced
    [Authorize]
    public class ServiceRequestController : Controller
    {
        // db = talk to sql tables, workflow = my brief rule checker (expired/on hold contracts)
        private readonly GlmsDbContext _db;
        private readonly ServiceRequestWorkflowService _workflow;

        // asp.net injects these automatically bc i registered them in Program.cs
        public ServiceRequestController(GlmsDbContext db, ServiceRequestWorkflowService workflow)
        {
            _db = db;
            _workflow = workflow;
        }

        // when user clicks "New request" link its a GET - no form data yet, i just show the page
        // url ends up as /ServiceRequest/Create (NOT ServiceRequests - class name is ServiceRequestController so drop Controller + no s on ServiceRequest)
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateServiceRequestViewModel();
            await PopulateContractDropdownAsync(model);
            return View(model);
        }

        // when user hits Save on the form the browser does a POST with the fields - same method name Create but different signature = overload
        [HttpPost]
        [ValidateAntiForgeryToken] // stops random sites posting to my url - needs matching token in the form
        public async Task<IActionResult> Create(CreateServiceRequestViewModel model)
        {
            // data annotations on the view model (Required, StringLength etc) fill ModelState automatically
            if (!ModelState.IsValid)
            {
                await PopulateContractDropdownAsync(model);
                return View(model);
            }

            // brief rule: dont save if parent contract is Expired or OnHold - workflow throws if blocked
            try
            {
                await _workflow.EnsureCanCreateServiceRequestAsync(model.ContractId);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateContractDropdownAsync(model);
                return View(model);
            }

            // map view model -> real entity that ef knows about, then insert row
            var entity = new ServiceRequest
            {
                ContractId = model.ContractId,
                Description = model.Description,
                Cost = model.Cost,
                Status = model.Status
            };
            _db.ServiceRequests.Add(entity);
            await _db.SaveChangesAsync(); // this is when sql actually runs

            TempData["Message"] = "Service request saved."; // flash message once after redirect
            return RedirectToAction(nameof(Create)); // PRG pattern - stops double submit if user refreshes
        }

        // helper so i dont copy paste the dropdown code in GET and POST
        // ViewBag is a dynamic bag i pass to the view - here i stick the SelectList for the <select>
        private async Task PopulateContractDropdownAsync(CreateServiceRequestViewModel vm)
        {
            var contracts = await _db.Contracts
                .Include(c => c.Client) // need client name in the label text
                .OrderByDescending(c => c.ContractId)
                .Select(c => new
                {
                    c.ContractId,
                    Label = "#" + c.ContractId + " — " + c.Client.Name + " (" + c.Status + ")"
                })
                .ToListAsync();

            // if ContractId still 0 (first load) pre-pick first contract in list so dropdown isnt empty weirdness
            vm.ContractId = vm.ContractId == 0 && contracts.Any() ? contracts.First().ContractId : vm.ContractId;
            ViewBag.ContractId = new SelectList(contracts, "ContractId", "Label", vm.ContractId);
        }
    }
}