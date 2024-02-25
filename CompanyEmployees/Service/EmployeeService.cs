using AutoMapper;
using Contracts;
using Entities.Exceptions;
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
}