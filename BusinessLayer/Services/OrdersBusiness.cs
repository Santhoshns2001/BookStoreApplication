using BusinessLayer.Interfaces;
using RepositaryLayer.Entities;
using RepositaryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class OrdersBusiness:IOrderBuss
    {
        private readonly IOrderRepo orderRepo;

        public OrdersBusiness(IOrderRepo orderRepo)
        {
            this.orderRepo = orderRepo;
        }

       public bool CancelOrder(int userid, int orderid)
        {
           return orderRepo.CancelOrder(userid, orderid);
        }

       public Orders PlaceOrder(int userId, int cartid, int addressid)
        {
           return orderRepo.PlaceOrder(userId, cartid,addressid);
        }

       public List<Orders> ViewAllOrders()
        {
           return orderRepo.ViewAllOrders();
        }

      public Orders ViewOrdersByOrderId(int orderId)
        {
           return orderRepo.ViewOrdersByOrderId(orderId);
        }

       public List<Orders> ViewOrdersByUserId(int userid)
        {
            return orderRepo.ViewOrdersByUserId(userid);
        }
    }
}
