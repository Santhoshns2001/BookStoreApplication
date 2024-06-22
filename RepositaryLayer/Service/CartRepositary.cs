using Microsoft.Extensions.Configuration;
using ModelLayer;
using RepositaryLayer.Entities;
using RepositaryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositaryLayer.Service
{
    public class CartRepositary :ICartRepo
    {
        private readonly SqlConnection conn = new SqlConnection();

        private readonly string sqlConnectionString;

        private readonly IConfiguration configuration;
        public CartRepositary(IConfiguration configuration)
        {
            this.configuration = configuration;
            sqlConnectionString = configuration.GetConnectionString("DbConnection");
            conn.ConnectionString = sqlConnectionString;
        }
        

        public Cart AddBookToCart(int bookid,int userid)
        {
            Cart cart = null;
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_AddBookToCart",conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", bookid);
                    cmd.Parameters.AddWithValue("@UserId", userid);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        cart = new Cart()
                        {
                            CartId = (int)reader["CartId"],
                            UserId= (int)reader["UserId"],
                            BookId = (int)reader["BookId"],
                            Title=(string)reader["Title"],
                            Author=(string)reader["Author"],
                            Image= (string)reader["Image"],
                            Quantity = (int)reader["Quantity"],
                            OriginalBookPrice = (int)reader["OriginalBookPrice"],
                            FinalBookPrice = (int)reader["FinalBookPrice"]
                        };
                        return cart;
                    }
                    return null;
                }
                else
                {
                    throw new Exception("connection was not established ");
                }

            }
            catch (Exception ex) { throw ex; }
            finally { conn.Close(); }

        }

        public List<Cart> ViewCartsByUser(int userid)
        {
            List<Cart> carts = new List<Cart>();
            
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_ViewCartsByUser", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userid);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Cart cart = new Cart()
                        {
                            CartId = (int)reader["CartId"],
                            UserId = (int)reader["UserId"],
                            BookId = (int)reader["BookId"],
                            Title = (string)reader["Title"],
                            Author = (string)reader["Author"],
                            Image = (string)reader["Image"],
                            Quantity = (int)reader["Quantity"],
                            OriginalBookPrice = (int)reader["OriginalBookPrice"],
                            FinalBookPrice = (int)reader["FinalBookPrice"]
                        };
                        carts.Add(cart);
                    }
                    return carts;
                }
                else
                {
                    throw new Exception("connection was not established");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { conn.Close(); }
        }


        public Cart UpdateCart(int cartId,int quantity)
        {
            Cart cart = null;
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_UpdateCart", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CartId", cartId);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                         cart = new Cart()
                        {
                            CartId = (int)reader["CartId"],
                            UserId = (int)reader["UserId"],
                            BookId = (int)reader["BookId"],
                            Title = (string)reader["Title"],
                            Author = (string)reader["Author"],
                            Image = (string)reader["Image"],
                            Quantity = (int)reader["Quantity"],
                            OriginalBookPrice = (int)reader["OriginalBookPrice"],
                            FinalBookPrice = (int)reader["FinalBookPrice"]
                        };
                        return cart;
                    }
                    return null;
                }
                else
                {
                    throw new Exception("connection was not established");
                }

            }catch(Exception ex) { throw ex; }
            finally { conn.Close(); }
        }


        public bool RemoveCart(int CartId)
        {
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_RemoveBookFromCart", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CartId", CartId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                else
                {
                    throw new Exception("connection was not established ");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { conn.Close(); }
        }

        public List<Cart> ViewAllCarts()
        {
            List<Cart> carts = new List<Cart>(); 
            try
            {
                if (conn != null) 
                {
                    SqlCommand cmd = new SqlCommand("usp_ViewAllCarts", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Cart cart = new Cart()
                        {
                            CartId = (int)reader["CartId"],
                            UserId = (int)reader["UserId"],
                            BookId = (int)reader["BookId"],
                            Title = (string)reader["Title"],
                            Author = (string)reader["Author"],
                            Image = (string)reader["Image"],
                            Quantity = (int)reader["Quantity"],
                            OriginalBookPrice = (int)reader["OriginalBookPrice"],
                            FinalBookPrice = (int)reader["FinalBookPrice"]
                        };
                        carts.Add(cart);
                    }
                    return carts;
                }
                else
                {
                    throw new Exception("connection was not established");
                }
            }
            catch (Exception ex) { throw ex; }
            finally { conn.Close(); }
        }

        public int NoOfBooksInUserCart(int userid)
        {
           return ViewCartsByUser(userid).Count();

        }

        public List<CartDetails> GetCartDetailsWithUsers()
        {
            List<CartDetails> cartdetails = new List<CartDetails>();
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_GetCartDetailsWithUsers", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        CartDetails cart = new CartDetails()
                        {
                            FullName = (string)reader["FullName"],
                            Email = (string)reader["Email"],
                            CartId = (int)reader["CartId"],
                            UserId = (int)reader["UserId"],
                            BookId = (int)reader["BookId"],
                            Title = (string)reader["Title"],
                            Author = (string)reader["Author"],
                            Image = (string)reader["Image"],
                            Quantity = (int)reader["Quantity"],
                            OriginalBookPrice = (int)reader["OriginalBookPrice"],
                            FinalBookPrice = (int)reader["FinalBookPrice"]
                        };
                        cartdetails.Add(cart);
                    }
                    return cartdetails;
                }
                else
                {
                    throw new Exception("connection was not established");
                }
            }
            catch (Exception ex) { throw ex; }
            finally { conn.Close(); }
        }


    }
}
