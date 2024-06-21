using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositaryLayer.Entities;

namespace BookStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderBuss orderBuss;

        public OrdersController(IOrderBuss orderBuss)
        {
            this.orderBuss = orderBuss;
        }

        [Authorize]
        [HttpPost]
        [Route("PlaceOrder")]
        public IActionResult PlaceOrder( int cartid)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            Orders order = orderBuss.PlaceOrder(userId, cartid);

            if (order != null)
            {
                return Ok(new ResponseModel<Orders> { IsSuccuss = true, Message = "order placed succussfully", Data = order });
            }
            else
            {
                return BadRequest(new ResponseModel<Orders> { IsSuccuss = false, Message = " failed to order book", Data = order });
            }

        }
        [Authorize]
        [HttpGet]
        [Route("ViewAllOrders")]
        public IActionResult ViewAllOrders()
        {
            List<Orders> orders = orderBuss.ViewAllOrders();

            if (orders != null)
            {
                return Ok(new ResponseModel<List<Orders>> { IsSuccuss = true, Message = "list of orders fetched succussfully", Data = orders });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = "something went wrong" });
            }
        }

        [Authorize]
        [HttpGet("ViewOrdersByUserId")]
        public IActionResult ViewOrdersByUserId()
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            var orders = orderBuss.ViewOrdersByUserId(userId);
            if (orders != null)
            {
                return Ok(new ResponseModel<List<Orders>> { IsSuccuss = true, Message = "orders fetched succussfully", Data = orders });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch orders", Data = "something went wrong  " });
            }
        }

        [Authorize]
        [HttpGet("ViewOrdersByOrderId")]
        public IActionResult ViewOrdersByOrderId(int orderid)
        {
            Orders order = orderBuss.ViewOrdersByOrderId(orderid);
            if (order != null)
            {
                return Ok(new ResponseModel<Orders> { IsSuccuss = true, Message = "order fetched  succussfully", Data = order });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = " no matched input" });
            }

        }
        [Authorize]
        [HttpDelete("CancelOrder")]
        public IActionResult CancelOrder(int orderid)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            bool res = orderBuss.CancelOrder(userId, orderid);
            if (res)
            {
                return Ok(new ResponseModel<string> { IsSuccuss = true, Message = "order is been canceled", Data = "order canceled succussfully" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to canceled order", Data = "something went wrong " });
            }

        }


    }
}
