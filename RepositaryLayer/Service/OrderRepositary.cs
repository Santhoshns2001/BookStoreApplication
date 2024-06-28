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
    public class OrderRepositary:IOrderRepo
    {
        private readonly SqlConnection conn = new SqlConnection();

        private readonly string sqlConnectionString;

        private readonly IConfiguration configuration;

        public OrderRepositary(IConfiguration configuration)
        {
            this.configuration = configuration;
            sqlConnectionString = configuration.GetConnectionString("DbConnection");
            conn.ConnectionString = sqlConnectionString;
        }

        public Orders PlaceOrder(int userId, int cartid, int addressid)
        {
            Orders order = null;
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_PlaceOrder", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@CartId", cartid);
                    cmd.Parameters.AddWithValue("@AddressId", addressid);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        order = new Orders()
                        {
                            OrderId = (int)reader["OrderId"],
                            
                            UserId= (int)reader["UserId"],
                            AddressId= (int)reader["AddressId"],
                            BookId= (int)reader["UserId"],
                            Title=(string)reader["Title"],
                            Author= (string)reader["Author"],
                            Image = (string)reader["Image"],
                            Quantity = (int)reader["Quantity"],
                            TotalOriginalBookPrice = (int)reader["TotalOriginalBookPrice"],
                            TotalFinalBookPrice = (int)reader["TotalFinalBookPrice"],
                            OrderDateTime = (DateTime)reader["OrderDateTime"],
                            IsDeleted = (bool)reader["IsDeleted"]
                        };
                        return order;
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

        public List<Orders> ViewAllOrders()
        {
            List<Orders> orders = new List<Orders>();
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_ViewAllOrders", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Orders order = new Orders()
                        {
                            OrderId = (int)reader["OrderId"],
                            AddressId = (int)reader["AddressId"],
                            UserId = (int)reader["UserId"],
                            BookId = (int)reader["UserId"],
                            Title = (string)reader["Title"],
                            Author = (string)reader["Author"],
                            Image = (string)reader["Image"],
                            Quantity = (int)reader["Quantity"],
                            TotalOriginalBookPrice = (int)reader["TotalOriginalBookPrice"],
                            TotalFinalBookPrice = (int)reader["TotalFinalBookPrice"],
                            OrderDateTime = (DateTime)reader["OrderDateTime"],
                            IsDeleted = (bool)reader["IsDeleted"]
                        };
                        orders.Add(order);
                    }
                    return orders;
                }
                else
                {
                    throw new Exception("connection was not established");
                }
            }
            catch (Exception ex) { throw ex; }
            finally { conn.Close(); }
        }

        public List<Orders> ViewOrdersByUserId(int userid)
        {
            List<Orders> orders = new List<Orders>();

            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_ViewOrdersByUserId", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userid);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Orders order = new Orders()
                        {
                            OrderId = (int)reader["OrderId"],
                            AddressId = (int)reader["AddressId"],
                            UserId = (int)reader["UserId"],
                            BookId = (int)reader["UserId"],
                            Title = (string)reader["Title"],
                            Author = (string)reader["Author"],
                            Image = (string)reader["Image"],
                            Quantity = (int)reader["Quantity"],
                            TotalOriginalBookPrice = (int)reader["TotalOriginalBookPrice"],
                            TotalFinalBookPrice = (int)reader["TotalFinalBookPrice"],
                            OrderDateTime = (DateTime)reader["OrderDateTime"],
                            IsDeleted = (bool)reader["IsDeleted"]
                        };
                        orders.Add(order);
                    }
                    return orders;
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

        public Orders ViewOrdersByOrderId(int orderId)
        {
            Orders order = null;
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_ViewOrdersByOrderId", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                         order = new Orders()
                        {
                            OrderId = (int)reader["OrderId"],
                            UserId = (int)reader["UserId"],
                             AddressId = (int)reader["AddressId"],
                             BookId = (int)reader["UserId"],
                            Title = (string)reader["Title"],
                            Author = (string)reader["Author"],
                            Image = (string)reader["Image"],
                            Quantity = (int)reader["Quantity"],
                            TotalOriginalBookPrice = (int)reader["TotalOriginalBookPrice"],
                            TotalFinalBookPrice = (int)reader["TotalFinalBookPrice"],
                            OrderDateTime = (DateTime)reader["OrderDateTime"],
                            IsDeleted = (bool)reader["IsDeleted"]
                        };
                       
                    }
                    return order;
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


        public bool CancelOrder(int userid, int orderid)
        {
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_CancelOrder", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userid);
                    cmd.Parameters.AddWithValue("@OrderId", orderid);
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


    }
}
