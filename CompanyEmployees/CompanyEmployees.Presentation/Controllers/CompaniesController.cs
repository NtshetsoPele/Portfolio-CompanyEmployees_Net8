using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[ApiController, Route(template: "api/[controller]")]
public class CompaniesController(IServiceManager service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var companies =
            await service.CompanyService.GetAllCompaniesAsync(trackChanges: false);
        return Ok(companies);
    }

    [HttpGet(template: "{id:guid}", Name = "CompanyById")]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var company = await service.CompanyService.GetCompanyAsync(id, trackChanges: false);
        return Ok(company);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
    {
        var createdCompany = await service.CompanyService.CreateCompanyAsync(company);
        return CreatedAtRoute(
            "CompanyById",
            new { id = createdCompany.Id },
            createdCompany);
    }

    [HttpGet(template: "collection/({ids})", Name = "CompanyCollection")]
    public async Task<IActionResult> GetCompanyCollection(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var companies = await service.CompanyService.GetByIdsAsync(ids, trackChanges: false);
        return Ok(companies);
    }

    [HttpPost(template: "collection")]
    public async Task<IActionResult> CreateCompanyCollection(
        [FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
    {
        var (companies, ids) = 
            await service.CompanyService.CreateCompanyCollectionAsync(companyCollection);
        return CreatedAtRoute("CompanyCollection", new { ids }, companies);
    }

    [HttpDelete(template: "{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        await service.CompanyService.DeleteCompanyAsync(id, trackChanges: false);
        return NoContent();
    }

    [HttpPut(template: "{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
    {
        await service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);
        return NoContent();
    }
}