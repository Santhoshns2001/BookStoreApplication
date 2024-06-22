using BusinessLayer.Interfaces;
using ModelLayer;
using RepositaryLayer.Entities;
using RepositaryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CartBusiness : ICartBuss
    {
        private readonly ICartRepo cartRepo;

        public CartBusiness(ICartRepo cartRepo)
        {
            this.cartRepo = cartRepo;
        }

      public  Cart AddBookToCart(int bookid, int userid)
        {
           return cartRepo.AddBookToCart(bookid, userid);
        }

       public List<Cart> ViewCartsByUser(int userid)
        {
            return cartRepo.ViewCartsByUser(userid);
        }

       public Cart UpdateCart(int cartId, int quantity)
        {
            return cartRepo.UpdateCart(cartId, quantity);
        }

       public bool RemoveCart(int CartId)
        {
           return cartRepo.RemoveCart(CartId);
        }

      public  List<Cart> ViewAllCarts()
        {
            return cartRepo.ViewAllCarts();
        }

      public  int NoOfBooksInUserCart(int userid)
        {
            return cartRepo.NoOfBooksInUserCart(userid);
        }

       public List<CartDetails> GetCartDetailsWithUsers()
        {
            return cartRepo.GetCartDetailsWithUsers();
        }
    }
}
