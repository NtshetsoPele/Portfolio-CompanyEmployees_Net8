﻿using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class EmployeeRepository(DbContext repositoryContext)
    : RepositoryBase<Employee>(repositoryContext), IEmployeeRepository
{
    public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges) =>
        [.. FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).OrderBy(e => e.Name)];

    public Employee? GetEmployee(Guid companyId, Guid id, bool trackChanges) =>
        FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges).SingleOrDefault();

    public void CreateEmployeeForCompany(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployee(Employee employee) => Delete(employee);
}