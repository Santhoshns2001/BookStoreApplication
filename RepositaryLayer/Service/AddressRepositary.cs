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
    public class AddressRepositary :IAddressRepo
    {
        private readonly SqlConnection conn = new SqlConnection();

        private readonly string sqlConnectionString;

        private readonly IConfiguration configuration;

        public AddressRepositary(IConfiguration configuration)
        {
            this.configuration = configuration;
            sqlConnectionString = configuration.GetConnectionString("DbConnection");
            conn.ConnectionString = sqlConnectionString;
        }



        public Addresses AddAddress(int userId, AddressModel addressModel)
        {
            Addresses adresses = null;
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_AddAddress", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@FullName", addressModel.FullName);
                    cmd.Parameters.AddWithValue("@Mobile", addressModel.Mobile);
                    cmd.Parameters.AddWithValue("@Address", addressModel.Address);
                    cmd.Parameters.AddWithValue("@City", addressModel.City);
                    cmd.Parameters.AddWithValue("@State", addressModel.State);
                    cmd.Parameters.AddWithValue("@Type", addressModel.Type);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        adresses = new Addresses()
                        {
                            AddressId = (int)reader["AddressId"],
                            UserId = (int)reader["UserId"],
                            FullName= (string)reader["FullName"],
                            Mobile = (long)reader["Mobile"],
                            Address= (string)reader["Address"],
                            City = (string)reader["City"],
                            State = (string)reader["State"],
                            Type = (string)reader["Type"]
                        };
                        return adresses;
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

        public List<Addresses> GetAllAddresses()
        {
            List<Addresses> addresses = new List<Addresses>();
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_GetAllAddresses", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Addresses address = new Addresses()
                        {
                            AddressId = (int)reader["AddressId"],
                            UserId = (int)reader["UserId"],
                            FullName = (string)reader["FullName"],
                            Mobile = (long)reader["Mobile"],
                            Address = (string)reader["Address"],
                            City = (string)reader["City"],
                            State = (string)reader["State"],
                            Type = (string)reader["Type"]
                        };
                        addresses.Add(address);
                    }
                    return addresses;
                }
                else
                {
                    throw new Exception("connection was not established");
                }
            }
            catch (Exception ex) { throw ex; }
            finally { conn.Close(); }
        }

        public List<Addresses> GetAddressesByUser(int userid)
        {
            List<Addresses> addresses = new List<Addresses>();

            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_GetAddressesByUserId", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userid);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Addresses address = new Addresses()
                        {
                            AddressId = (int)reader["AddressId"],
                            UserId = (int)reader["UserId"],
                            FullName = (string)reader["FullName"],
                            Mobile = (long)reader["Mobile"],
                            Address = (string)reader["Address"],
                            City = (string)reader["City"],
                            State = (string)reader["State"],
                            Type = (string)reader["Type"]
                        };
                        addresses.Add(address);
                    }
                    return addresses;
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

        public List<Addresses> GetAddressesByUserIdAndAddressId(int userid,int addressId)
        {
            List<Addresses> addresses = new List<Addresses>();

            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_GetAddressByUserIdAndAddressId", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userid);
                    cmd.Parameters.AddWithValue("@AddressId", addressId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Addresses address = new Addresses()
                        {
                            AddressId = (int)reader["AddressId"],
                            UserId = (int)reader["UserId"],
                            FullName = (string)reader["FullName"],
                            Mobile = (long)reader["Mobile"],
                            Address = (string)reader["Address"],
                            City = (string)reader["City"],
                            State = (string)reader["State"],
                            Type = (string)reader["Type"]
                        };
                        addresses.Add(address);
                    }
                    return addresses;
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

        public Addresses UpdateAddress(int userid, int addressid,AddressModel addressModel)
        {
            Addresses address = null;
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_UpdateAddress", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userid);
                    cmd.Parameters.AddWithValue("@AddressId", addressid);
                    cmd.Parameters.AddWithValue("@FullName", addressModel.FullName);
                    cmd.Parameters.AddWithValue("@Mobile", addressModel.Mobile);
                    cmd.Parameters.AddWithValue("@Address", addressModel.Address);
                    cmd.Parameters.AddWithValue("@City", addressModel.City);
                    cmd.Parameters.AddWithValue("@State", addressModel.State);
                    cmd.Parameters.AddWithValue("@Type", addressModel.Type);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        address = new Addresses()
                        {
                            AddressId = (int)reader["AddressId"],
                            UserId = (int)reader["UserId"],
                            FullName = (string)reader["FullName"],
                            Mobile = (long)reader["Mobile"],
                            Address = (string)reader["Address"],
                            City = (string)reader["City"],
                            State = (string)reader["State"],
                            Type = (string)reader["Type"]
                        };
                        return address;
                    }
                    return null;
                }
                else
                {
                    throw new Exception("connection was not established");
                }

            }
            catch (Exception ex) { throw ex; }
            finally { conn.Close(); }
        }

        public bool RemoveAddress(int userid,int addressId)
        {
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_DeleteAddress", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userid);
                    cmd.Parameters.AddWithValue ("@AddressId", addressId);
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
