using Bulky.Models;

namespace Bulky.DataAccess.Repository.iRepository;

public interface iProductRepository: iRepository<Product>
{
    void Update(Product obj);
    void Save();
    
    
}