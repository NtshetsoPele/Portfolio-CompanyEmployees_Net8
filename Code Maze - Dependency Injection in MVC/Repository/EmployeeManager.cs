using Contracts;
using Entities.Models;

namespace Repository
{
    public class EmployeeManager : IDataRepository<Employee>
    {
        public void Add(Employee employee)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Employee> IDataRepository<Employee>.GetAll()
        {
            return new List<Employee>()
            {
                new Employee() { }
            };
        }
    }
}
