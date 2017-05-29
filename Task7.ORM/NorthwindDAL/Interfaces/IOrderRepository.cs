using System.Collections.Generic;
using NorthwindORM;

namespace NorthwindDAL.Interfaces
{
    public interface IOrderRepository
    {
        Order Get(int id);

        void ChangeProduct(IEnumerable<Order> orders, int oldProductId, int newProductId);
    }
}
