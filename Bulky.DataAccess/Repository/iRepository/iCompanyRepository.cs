using Bulky.Models;

namespace Bulky.DataAccess.Repository.iRepository;

public interface iCompanyRepository: iRepository<Company>
{
    void Update(Company obj);
    void Save();




}