using Bulky.Models;

namespace Bulky.DataAccess.Repository.iRepository;

public interface iApplicationUserRepository: iRepository<ApplicationUser>
{

    void Save();




}