using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.iRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository;

public class OrderHeaderRepository: Repository<OrderHeader>, iOrderHeaderRepository
{
    private ApplicationDbContext _db;//we are creating a private variable of type ApplicationDbContext
    public OrderHeaderRepository(ApplicationDbContext db):base(db)//base(db) means we are passing the db to the base class
    {
        _db = db;//we are assigning the db to the private variable
    }
  

    public void Update(OrderHeader obj)
    {
        _db.OrderHeaders.Update(obj);
    }
    
    public void UpdateStatus(int id, string orderStatus, string? paymentStatus=null)
    {
        var objFromDb = _db.OrderHeaders.FirstOrDefault(s => s.Id == id);
        
        if (objFromDb != null)
        {
            objFromDb.OrderStatus = orderStatus;
            if (paymentStatus != null)
            {
                objFromDb.PaymentStatus = paymentStatus;
            }
        }
      
    }
    
    
    public void UpdateStripePaymentID(int id, string? sessionId, string? paymentIntentId)
    {
        var objFromDb = _db.OrderHeaders.FirstOrDefault(s => s.Id == id);

        if (objFromDb != null)
        {
            if(sessionId != null)
            {
                objFromDb.SessionId = sessionId;
            }
            if(paymentIntentId != null)
            {
                objFromDb.PaymentIntentID= paymentIntentId;
                objFromDb.PaymentDate= DateTime.Now;
            }
            
        }
        
   

       

        
        
   
      

     
       
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}