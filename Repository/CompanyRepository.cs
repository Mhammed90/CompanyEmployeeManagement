using Contracts;
using Entities.Models;

namespace Repository;

public class CompanyRepository:RepositoryBase<Company>,ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public IEnumerable<Company> GetAllCompanies(bool trackChanges)=> 
    FindAll(trackChanges).OrderBy(n=>n.Name).ToList();
}