using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[ApiController, Route(template: "api/companies/{companyId:guid}/[controller]")]
public class EmployeesController(IServiceManager service) : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployeesForCompany(Guid companyId)
    {
        var employees = service.EmployeeService.GetEmployees(companyId, trackChanges: false);
        return Ok(employees);
    }

    [HttpGet(template: "{employeeId:guid}", Name = "GetEmployeeForCompany")]
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid employeeId)
    {
        var employee = service.EmployeeService.GetEmployee(companyId, employeeId, trackChanges: false);
        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
    {
        if (employee is null)
        {
            return BadRequest("EmployeeForCreationDto object is null");
        }

        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        
        var employeeToReturn =
            service.EmployeeService.CreateEmployeeForCompany(companyId, employee, trackChanges: false);
        
        return CreatedAtRoute(
            "GetEmployeeForCompany", 
            new
            {
                companyId,
                employeeId = employeeToReturn.Id
            },
            employeeToReturn);
    }

    [HttpDelete(template: "{id:guid}")]
    public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        service.EmployeeService.DeleteEmployeeForCompany(companyId, id, trackChanges: false);
        return NoContent();
    }

    // Full update
    [HttpPut(template: "{id:guid}")]
    public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
    {
        if (employee is null)
        {
            return BadRequest("EmployeeForUpdateDto object is null");
        }

        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        
        service.EmployeeService.UpdateEmployeeForCompany(companyId, id, employee, compTrackChanges: false, empTrackChanges: true);
        return NoContent();
    }

    [HttpPatch(template: "{id:guid}")]
    public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        if (patchDoc is null)
        {
            return BadRequest("patchDoc object sent from client is null.");
        }
        var (employeeToPatch, employeeEntity) = 
            service.EmployeeService.GetEmployeeForPatch(companyId, id, compTrackChanges: false, empTrackChanges: true);
        patchDoc.ApplyTo(employeeToPatch, ModelState);
        TryValidateModel(employeeToPatch);
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        service.EmployeeService.SaveChangesForPatch(employeeToPatch, employeeEntity);
        return NoContent();
    }
}