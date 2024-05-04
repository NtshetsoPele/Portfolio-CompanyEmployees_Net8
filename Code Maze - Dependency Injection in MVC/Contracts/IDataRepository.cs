using Entities.Models;

namespace Contracts;

public interface IDataRepository<TEntity>
{
    IEnumerable<TEntity> GetAll();
    void Add(Employee employee);
}