using RepositaryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositaryLayer.Interfaces
{
    public interface IWishlistRepo
    {
        public Wishlist AddBookToWishlist(int bookid, int userid);
        public List<Wishlist> ViewWishlistsByUser(int userid);
        bool RemoveFromWishlist(int wishlistId);
        public List<Wishlist> ViewAllWishlists();

    }
}
