using RepositaryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IOrderBuss
    {
        public Orders PlaceOrder(int userId, int cartid);
        public List<Orders> ViewAllOrders();
        public List<Orders> ViewOrdersByUserId(int userid);
        public Orders ViewOrdersByOrderId(int orderId);
        public bool CancelOrder(int userid, int orderid);
    }
}
