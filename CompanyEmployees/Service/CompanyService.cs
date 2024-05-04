using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapperBase mapper) : ICompanyService
{
    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
    {
        var companies = await repository.Company.GetAllCompaniesAsync(trackChanges);
        var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companiesDto;
    }

    /// <exception cref="CompanyNotFoundException">Condition.</exception>
    public async Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(id, trackChanges) ?? throw new CompanyNotFoundException(id);
        var companyDto = mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
    {
        var companyEntity = mapper.Map<Company>(company);
        repository.Company.CreateCompany(companyEntity);
        await repository.SaveAsync();
        return mapper.Map<CompanyDto>(companyEntity);
    }

    /// <exception cref="IdParametersBadRequestException">Condition.</exception>
    /// <exception cref="CollectionByIdsBadRequestException">Condition.</exception>
    public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
        {
            throw new IdParametersBadRequestException();
        }
        var companyIds = ids as Guid[] ?? ids.ToArray();
        var companyEntities = await repository.Company.GetByIdsAsync(companyIds, trackChanges);
        if (companyIds.Length != companyEntities.Count())
        {
            throw new CollectionByIdsBadRequestException();
        }
        var companiesToReturn = mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        return companiesToReturn;
    }

    /// <exception cref="CompanyCollectionBadRequest">Condition.</exception>
    /// <exception cref="OutOfMemoryException">The length of the resulting string overflows the maximum allowed length (<see cref="System.Int32.MaxValue">Int32.MaxValue</see>).</exception>
    public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(
        IEnumerable<CompanyForCreationDto> companyCollection)
    {
        if (companyCollection is null)
        {
            throw new CompanyCollectionBadRequest();
        }
        var companyEntities = mapper.Map<IEnumerable<Company>>(companyCollection);
        var entities = companyEntities as Company[] ?? companyEntities.ToArray();
        foreach (var company in entities)
        {
            repository.Company.CreateCompany(company);
        }
        await repository.SaveAsync();
        var companyCollectionToReturn = mapper.Map<IEnumerable<CompanyDto>>(entities);
        var companies = companyCollectionToReturn as CompanyDto[] ?? companyCollectionToReturn.ToArray();
        var ids = string.Join(",", companies.Select(c => c.Id));
        return (companies, ids);
    }

    /// <exception cref="CompanyNotFoundException">Condition.</exception>
    public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        repository.Company.DeleteCompany(company);
        await repository.SaveAsync();
    }

    /// <exception cref="CompanyNotFoundException">Condition.</exception>
    public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
    {
        var companyEntity = await repository.Company.GetCompanyAsync(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        mapper.Map(companyForUpdate, companyEntity);
        await repository.SaveAsync();
    }
}