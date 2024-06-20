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
    public class WishlistBusiness : IWishlistBuss
    {
        private readonly IWishlistRepo wishlistRepo;

        public WishlistBusiness(IWishlistRepo wishlistRepo)
        {
            this.wishlistRepo = wishlistRepo;
        }

        public Wishlist AddBookToWishlist(int bookid, int userid)
        {
            return wishlistRepo.AddBookToWishlist(bookid, userid);
        }

       public List<Wishlist> ViewWishlistsByUser(int userid)
        {
           return wishlistRepo.ViewWishlistsByUser(userid);
        }

       public bool RemoveFromWishlist(int WishlistId)
        {
           return wishlistRepo.RemoveFromWishlist(WishlistId);
        }

       public List<Wishlist> ViewAllWishlists()
        {
            return wishlistRepo.ViewAllWishlists();
        }
    }
}
