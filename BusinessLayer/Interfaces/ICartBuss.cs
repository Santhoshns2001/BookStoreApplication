using ModelLayer;
using RepositaryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ICartBuss
    {
        public Cart AddBookToCart(int bookid, int userid);
        public List<Cart> ViewCartsByUser(int userid);
        public Cart UpdateCart(int cartId, int quantity);
        public bool RemoveCart(int CartId);
        public List<Cart> ViewAllCarts();
        public int NoOfBooksInUserCart(int userid);
        public List<CartDetails> GetCartDetailsWithUsers();

    }
}
