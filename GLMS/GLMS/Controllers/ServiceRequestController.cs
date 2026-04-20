using GLMS.Data;
using GLMS.Models;
using GLMS.Services;
using GLMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Controllers;

// External API (section 3) flow: assisted by DeepSeek AI — i reviewed and integrated it.
[Authorize]
public class ServiceRequestController : Controller
{
    private readonly GlmsDbContext _db;
    private readonly ServiceRequestWorkflowService _workflow;
    private readonly UsdZarExchangeService _fx;

    public ServiceRequestController(
        GlmsDbContext db,
        ServiceRequestWorkflowService workflow,
        UsdZarExchangeService fx)
    {
        _db = db;
        _workflow = workflow;
        _fx = fx;
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CreateServiceRequestViewModel();
        await PopulateContractDropdownAsync(model);
        return View(model);
    }

    // JSON for the create page: JS calls here → server uses HttpClient (Frankfurter) → returns zarPerUsd
    [HttpGet]
    public async Task<IActionResult> UsdToZarRate(CancellationToken ct)
    {
        var rate = await _fx.GetUsdToZarAsync(ct);
        if (rate is null)
            return StatusCode(503, new { error = "Could not load USD/ZAR rate from external API." });

        return Json(new { zarPerUsd = rate.Value });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateServiceRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateContractDropdownAsync(model);
            return View(model);
        }

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

        // Re-fetch rate on save (don’t rely only on browser preview)
        var rate = await _fx.GetUsdToZarAsync(HttpContext.RequestAborted);
        if (rate is null)
        {
            ModelState.AddModelError(string.Empty, "Exchange rate unavailable. Cannot convert USD to ZAR right now.");
            await PopulateContractDropdownAsync(model);
            return View(model);
        }

        var costZar = Math.Round(model.CostUsd * rate.Value, 2, MidpointRounding.AwayFromZero);

        var entity = new ServiceRequest
        {
            ContractId = model.ContractId,
            Description = model.Description,
            Cost = costZar,
            Status = model.Status
        };
        _db.ServiceRequests.Add(entity);
        await _db.SaveChangesAsync();

        TempData["Message"] = "Service request saved.";
        return RedirectToAction(nameof(Create));
    }

    private async Task PopulateContractDropdownAsync(CreateServiceRequestViewModel vm)
    {
        var contracts = await _db.Contracts
            .Include(c => c.Client)
            .OrderByDescending(c => c.ContractId)
            .Select(c => new
            {
                c.ContractId,
                Label = "#" + c.ContractId + " — " + c.Client.Name + " (" + c.Status + ")"
            })
            .ToListAsync();

        vm.ContractId = vm.ContractId == 0 && contracts.Any() ? contracts.First().ContractId : vm.ContractId;
        ViewBag.ContractId = new SelectList(contracts, "ContractId", "Label", vm.ContractId);
    }
}
