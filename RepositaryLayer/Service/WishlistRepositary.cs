using Microsoft.Extensions.Configuration;
using RepositaryLayer.Entities;
using RepositaryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositaryLayer.Service
{
    public class WishlistRepositary:IWishlistRepo
    {
        private readonly SqlConnection conn = new SqlConnection();

        private readonly string sqlConnectionString;

        private readonly IConfiguration configuration;
        public WishlistRepositary(IConfiguration configuration)
        {
            this.configuration = configuration;
            sqlConnectionString = configuration.GetConnectionString("DbConnection");
            conn.ConnectionString = sqlConnectionString;
        }


        public Wishlist AddBookToWishlist(int bookid, int userid)
        {
            Wishlist wishlist = null;
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_AddToWishlist", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", bookid);
                    cmd.Parameters.AddWithValue("@UserId", userid);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        wishlist = new Wishlist()
                        {
                           WishlistId = (int)reader["WishlistId"],
                            UserId = (int)reader["UserId"],
                            BookId = (int)reader["BookId"],
                            Title = (string)reader["Title"],
                            Author = (string)reader["Author"],
                            Image = (string)reader["Image"],
                            OriginalBookPrice = (int)reader["OriginalBookPrice"],
                            FinalBookPrice = (int)reader["FinalBookPrice"]
                        };
                        return wishlist;
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

        public List<Wishlist> ViewWishlistsByUser(int userid)
        {
            List<Wishlist> wishlists = new List<Wishlist>();

            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_ViewWishlistsByUser", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userid);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Wishlist wishlist = new Wishlist()
                        {
                            WishlistId = (int)reader["WishlistId"],
                            UserId = (int)reader["UserId"],
                            BookId = (int)reader["BookId"],
                            Title = (string)reader["Title"],
                            Author = (string)reader["Author"],
                            Image = (string)reader["Image"],
                            OriginalBookPrice = (int)reader["OriginalBookPrice"],
                            FinalBookPrice = (int)reader["FinalBookPrice"]
                        };
                        wishlists.Add(wishlist);
                    }
                    return wishlists;
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

        public bool RemoveFromWishlist(int wishlistId)
        {
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_RemoveBookFromWishlist", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@WishlistId", wishlistId);
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

        public List<Wishlist> ViewAllWishlists()
        {
            List<Wishlist> wishlists = new List<Wishlist>();
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_ViewAllWishlist", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Wishlist wishlist = new Wishlist()
                        {
                            WishlistId = (int)reader["WishlistId"],
                            UserId = (int)reader["UserId"],
                            BookId = (int)reader["BookId"],
                            Title = (string)reader["Title"],
                            Author = (string)reader["Author"],
                            Image = (string)reader["Image"],
                            OriginalBookPrice = (int)reader["OriginalBookPrice"],
                            FinalBookPrice = (int)reader["FinalBookPrice"]
                        };
                        wishlists.Add(wishlist);
                    }
                    return wishlists;
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
