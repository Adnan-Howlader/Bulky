using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.iRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository;

public class OrderDetailRepository: Repository<OrderDetails>, iOrderDetailRepository
{
    private ApplicationDbContext _db;//we are creating a private variable of type ApplicationDbContext
    public OrderDetailRepository(ApplicationDbContext db):base(db)//base(db) means we are passing the db to the base class
    {
        _db = db;//we are assigning the db to the private variable
    }
  

    public void Update(OrderDetails obj)
    {
        _db.OrderDetails.Update(obj);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}