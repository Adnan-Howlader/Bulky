using Bulky.Models;

namespace Bulky.DataAccess.Repository.iRepository;

public interface iOrderHeaderRepository: iRepository<OrderHeader>
{
    void Update(OrderHeader obj);
    
    void UpdateStatus(int id, string orderStatus, string? paymentStatus=null);
    
    void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);
    void Save();




}