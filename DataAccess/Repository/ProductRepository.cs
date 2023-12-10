using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.iRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository;

public class ProductRepository: Repository<Product>, iProductRepository
{
    private ApplicationDbContext _db;
    public ProductRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
    
  

    public void Update(Product obj)
    {
        _db.Products.Update(obj);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}