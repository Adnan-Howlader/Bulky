using Bulky.Models;

namespace Bulky.DataAccess.Repository.iRepository;

public interface iOrderDetailRepository: iRepository<OrderDetails>
{
    void Update(OrderDetails obj);
    void Save();




}