using RepositaryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IWishlistBuss
    {
        public Wishlist AddBookToWishlist(int bookid, int userid);
        public List<Wishlist> ViewWishlistsByUser(int userid);
        public bool RemoveFromWishlist(int WishlistId);
        public List<Wishlist> ViewAllWishlists();
    }
}
