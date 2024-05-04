using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : IEmployeeService
{
    //private readonly IRepositoryManager _repository = repository;
    //private readonly ILoggerManager _logger = logger;

    /// <exception cref="CompanyNotFoundException">Condition.</exception>
    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        _ = repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employeesFromDb = repository.Employee.GetEmployees(companyId, trackChanges);
        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
        return employeesDto;
    }

    /// <exception cref="CompanyNotFoundException">Condition.</exception>
    /// <exception cref="EmployeeNotFoundException">Condition.</exception>
    public EmployeeDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        _ = repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employeeDb = repository.Employee.GetEmployee(companyId, employeeId, trackChanges) ?? throw new EmployeeNotFoundException(employeeId);
        var employee = mapper.Map<EmployeeDto>(employeeDb);
        return employee;
    }

    /// <exception cref="CompanyNotFoundException">Condition.</exception>
    public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
    {
        _ = repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employeeEntity = mapper.Map<Employee>(employeeForCreation);
        repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        repository.Save();
        return mapper.Map<EmployeeDto>(employeeEntity);
    }

    /// <exception cref="CompanyNotFoundException">Condition.</exception>
    /// <exception cref="EmployeeNotFoundException">Condition.</exception>
    public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
    {
        _ = repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employeeForCompany = repository.Employee.GetEmployee(companyId, id, trackChanges) ?? throw new EmployeeNotFoundException(id);
        repository.Employee.DeleteEmployee(employeeForCompany);
        repository.Save();
    }

    /// <exception cref="CompanyNotFoundException">Condition.</exception>
    /// <exception cref="EmployeeNotFoundException">Condition.</exception>
    public void UpdateEmployeeForCompany(
        Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
    {
        _ = repository.Company.GetCompany(companyId, compTrackChanges) ?? throw new CompanyNotFoundException(companyId);
        // Connected update
        var employeeEntity = repository.Employee.GetEmployee(companyId, id, empTrackChanges) ?? throw new EmployeeNotFoundException(id);
        mapper.Map(employeeForUpdate, employeeEntity);
        repository.Save();
    }

    /// <exception cref="CompanyNotFoundException">Condition.</exception>
    /// <exception cref="EmployeeNotFoundException">Condition.</exception>
    public (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) GetEmployeeForPatch(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
    {
        _ = repository.Company.GetCompany(companyId, compTrackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employeeEntity = repository.Employee.GetEmployee(companyId, id, empTrackChanges) ?? throw new EmployeeNotFoundException(companyId);
        var employeeToPatch = mapper.Map<EmployeeForUpdateDto>(employeeEntity);
        return (employeeToPatch, employeeEntity);
    }

    public void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        mapper.Map(employeeToPatch, employeeEntity);
        repository.Save();
    }
}