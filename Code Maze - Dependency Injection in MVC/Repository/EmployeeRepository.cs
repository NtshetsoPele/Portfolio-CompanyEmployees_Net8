using Contracts;
using Entities.Models;

namespace Repository;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
}