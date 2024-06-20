using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositaryLayer.Entities;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartBuss cartBuss;

        public CartController(ICartBuss cartBuss)
        {
            this.cartBuss = cartBuss;
        }


        [HttpPost("AddBookToCart")]
        public IActionResult AddBookToCart(int bookid, int userId)
        {
            if (userId == 0)
            {
                userId = Convert.ToInt32(User.FindFirst("userId").Value);
            }

            Cart cart = cartBuss.AddBookToCart(bookid, userId);
            if (cart != null)
            {
                return Ok(new ResponseModel<Cart> { IsSuccuss = true, Message = "book added to cart succussfully", Data = cart });
            }
            else
            {
                return BadRequest(new ResponseModel<Cart> { IsSuccuss = false, Message = " failed to add book to a cart", Data = cart });
            }

        }

        [HttpGet("GetAllCarts")]
        public IActionResult ViewCartsByUser(int userId)
        {
            var carts =cartBuss.ViewCartsByUser(userId);
            if (carts != null)
            {
                return Ok(new ResponseModel<List<Cart>> { IsSuccuss = true, Message = "list of books fetched succussfully", Data = carts });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = "something went wrong" });
            }
        }

        [HttpPut("UpdateCart")]
        public IActionResult UpdateCart(int cartId, int quantity)
        {
            Cart cart=cartBuss.UpdateCart(cartId,quantity);
            if (cart != null)
            {
                return Ok(new ResponseModel<Cart> { IsSuccuss = true, Message = "cart updated  succussfully", Data = cart });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to update", Data = "wrong input is been provided " });
            }
        }

        [HttpDelete("RemoveCart")]
        public ActionResult RemoveCart(int cartId)
        {
            var cart=cartBuss.RemoveCart(cartId);
            if (cart)
            {
                return Ok(new ResponseModel<string> { IsSuccuss = true, Message = "cart deleted  succussfully", Data = "deleted succussfully" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to delete", Data = "delete unsuccuss" });
            }

        }

        [HttpGet("ViewAllCarts")]
        public IActionResult ViewAllCarts()
        {
            var carts = cartBuss.ViewAllCarts();
            if (carts!=null)
            {
                return Ok(new ResponseModel<List<Cart>> { IsSuccuss = true, Message = "fetch all views is succuss", Data = carts });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = "fetch all carts operation is unsuccuss" });
            }

        }

        [HttpGet("NoOfBooksInUserCart")]
        public IActionResult NoOfBooksInUserCart(int userid)
        {
            int carts = cartBuss.NoOfBooksInUserCart(userid);
            if (carts != 0)
            {
                return Ok(new ResponseModel<int> { IsSuccuss = true, Message = "number of carts of the user is ", Data = carts });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to count the cart", Data = "count all carts operation is unsuccuss" });
            }

        }


    }
}
