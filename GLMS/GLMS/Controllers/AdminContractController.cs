using GLMS.Models;
using GLMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Controllers;

// admin contract search page - i use [Authorize] only bc negotiate on the lab vm doesnt give me proper "Admin" roles to test with
// if i used [Authorize(Roles = "Admin")] i would get 403 forever unless i configure AD group mapping (out of scope for this hand in)
[Authorize]
public class AdminContractsController : Controller
{
    private readonly ContractQueryService _queries;

    public AdminContractsController(ContractQueryService queries)
    {
        _queries = queries;
    }

    [HttpGet]
    public async Task<IActionResult> Index(ContractStatus? status, DateTime? from, DateTime? to)
    {
        ViewBag.Status = new SelectList(Enum.GetValues<ContractStatus>(), status);
        ViewBag.From = from?.ToString("yyyy-MM-dd");
        ViewBag.To = to?.ToString("yyyy-MM-dd");

        var list = await _queries
            .GetContractForAdmin(status, from, to)
            .ToListAsync();

        return View(list);
    }
}