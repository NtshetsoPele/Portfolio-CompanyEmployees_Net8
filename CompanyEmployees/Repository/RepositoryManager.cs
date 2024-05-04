using Contracts;

namespace Repository;

public sealed class RepositoryManager(RepositoryContext repositoryContext) : IRepositoryManager
{
    private readonly Lazy<ICompanyRepository> _companyRepository = new(() => new
        CompanyRepository(repositoryContext));
    private readonly Lazy<IEmployeeRepository> _employeeRepository = new(() => new
        EmployeeRepository(repositoryContext));

    public ICompanyRepository Company => _companyRepository.Value;
    
    public IEmployeeRepository Employee => _employeeRepository.Value;

    /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
    /// <exception cref="DbUpdateConcurrencyException">A concurrency violation is encountered while saving to the database.
    ///                 A concurrency violation occurs when an unexpected number of rows are affected during save.
    ///                 This is usually because the data in the database has been modified since it was loaded into memory.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task SaveAsync() => await repositoryContext.SaveChangesAsync();
}