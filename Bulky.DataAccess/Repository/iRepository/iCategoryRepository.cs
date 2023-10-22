using Bulky.Models;

namespace Bulky.DataAccess.Repository.iRepository;

public interface iCategoryRepository: iRepository<Category>
{
    void Update(Category obj);
    void Save();




}