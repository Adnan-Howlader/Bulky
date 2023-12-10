using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.iRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository;

public class CompanyRepository: Repository<Company>, iCompanyRepository
{
    private ApplicationDbContext _db;//we are creating a private variable of type ApplicationDbContext
    public CompanyRepository(ApplicationDbContext db):base(db)//base(db) means we are passing the db to the base class
    {
        _db = db;//we are assigning the db to the private variable
    }
  

    public void Update(Company obj)
    {
        _db.Companies.Update(obj);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}