using Bulky.Models;

namespace Bulky.DataAccess.Repository.iRepository;

public interface iShoppingCartRepository: iRepository<ShoppingCart>
{
    void Update(ShoppingCart obj);
    void Save();




}