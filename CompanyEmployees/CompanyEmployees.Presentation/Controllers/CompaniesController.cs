using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers;

[Route(template: "api/[controller]")]
[ApiController]
public class CompaniesController(IServiceManager service) : ControllerBase
{
    [HttpGet]
    public IActionResult GetCompanies()
    {
        //throw new Exception("Exception");
        var companies =
            service.CompanyService.GetAllCompanies(trackChanges: false);
        return Ok(companies);
    }

    [HttpGet(template: "{id:guid}")]
    public IActionResult GetCompany(Guid id)
    {
        var company = service.CompanyService.GetCompany(id, trackChanges: false);
        return Ok(company);
    }
}