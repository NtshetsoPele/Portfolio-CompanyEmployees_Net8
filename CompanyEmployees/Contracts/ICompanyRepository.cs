using Entities.Models;

namespace Contracts;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges);
    Task<Company?> GetCompanyAsync(Guid companyId, bool trackChanges);
    // Left synchronous because there's no I/O here. Entity state is just marked.
    void CreateCompany(Company company);
    Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    void DeleteCompany(Company company);
}