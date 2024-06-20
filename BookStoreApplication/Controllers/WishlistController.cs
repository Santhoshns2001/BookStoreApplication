using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositaryLayer.Entities;
using RepositaryLayer.Interfaces;

namespace BookStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistBuss wishlistBuss;

        public WishlistController(IWishlistBuss wishlistBuss)
        {
            this.wishlistBuss = wishlistBuss;
        }




        [HttpPost("AddingBookToWishlist")]
        public IActionResult AddBookToWishlist(int bookid,int userid)
        {
            Wishlist wishlist=wishlistBuss.AddBookToWishlist(bookid,userid);
            if (wishlist != null)
            {
                return Ok(new ResponseModel<Wishlist> { IsSuccuss = true, Message = "book added to wishlist succussfully", Data = wishlist });
            }
            else
            {
                return BadRequest(new ResponseModel<Wishlist> { IsSuccuss = false, Message = " failed to add book to a wishlist", Data = wishlist });
            }
        }


        [HttpGet("GetAllWishlists")]
        public IActionResult ViewWishlistsByUser(int userId)
        {
            var wishlists = wishlistBuss.ViewWishlistsByUser(userId);
            if (wishlists != null)
            {
                return Ok(new ResponseModel<List<Wishlist>> { IsSuccuss = true, Message = "list of wishlists fetched succussfully", Data = wishlists });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = "something went wrong" });
            }
        }

        [HttpDelete("RemoveFromWishlist")]
        public ActionResult RemoveFromWishlist(int wishlistid)
        {
            var wishlist = wishlistBuss.RemoveFromWishlist(wishlistid);
            if (wishlist)
            {
                return Ok(new ResponseModel<string> { IsSuccuss = true, Message = " deleted from wishlist  succussfully", Data = "deleted succussfully" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to delete", Data = "delete unsuccuss" });
            }

        }

        [HttpGet("ViewAllWishlists")]
        public IActionResult ViewAllWishlists()
        {
            var wishlists = wishlistBuss.ViewAllWishlists();
            if (wishlists != null)
            {
                return Ok(new ResponseModel<List<Wishlist>> { IsSuccuss = true, Message = "fetch all wishlists is succuss", Data = wishlists });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = "fetch all wishlists operation is unsuccuss" });
            }

        }



    }
}
