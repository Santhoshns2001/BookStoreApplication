using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using RepositaryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace RepositaryLayer.Service
{
    public class UserRepositary : IUserRepo
    {
        private readonly SqlConnection conn = new SqlConnection();

        private readonly string sqlConnectionString;
            
        private readonly IConfiguration configuration;


        public UserRepositary(IConfiguration configuration)
        {
            this.configuration = configuration;
            sqlConnectionString = configuration.GetConnectionString("DbConnection");
            conn.ConnectionString = sqlConnectionString;
        }
        

        public UserModel RegisterUser(UserModel user)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_UserRegister", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@fullName", user.fullName);
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@password", HashPassword(user.password));
                cmd.Parameters.AddWithValue("@mobile", user.mobile);
                conn.Open();
                cmd.ExecuteNonQuery();
                return user;
            }
            catch (Exception ex) { throw ex; }
            finally
            { conn.Close(); }

        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private string GenerateToken(string Email, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email",Email),
                new Claim("userId",userId.ToString())
            };
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string LoginUser(string email, string password)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_UserLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", HashPassword(password));
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var token = GenerateToken((string)reader["email"], (int)reader["userId"]);
                    
                    return token;
                }
                return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally { conn.Close(); }
        }

       public UserModel FetchByUSerId(int userId)
        {
            UserModel userModel = null;

            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_FetchByUserId", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        userModel = new UserModel()
                        {
                            
                            fullName = (string)reader["fullname"],
                            email = (string)reader["email"],
                            password = (string)reader["password"],
                            mobile = (long)reader["mobile"],
                        };
                    }
                    return userModel;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex) { throw ex; }
            finally { conn.Close(); }

        }

       public List<UserModel> FetchAllUsers()
        {
            List<UserModel> users=new List<UserModel> ();
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_FetchAllUsers", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        UserModel userModel = new UserModel()
                        {
                           
                            fullName = (string)reader["fullname"],
                            email = (string)reader["email"],
                            password = (string)reader["password"],
                            mobile = (long)reader["mobile"]
                        };
                        users.Add(userModel);
                    }
                    return users.ToList();
                }
                return null;
            }catch(Exception ex) { throw ex; }
            finally{ conn.Close(); }
            
        }

      public  UserModel UpdateUser(int userId, UserModel user)
        {
            try
            {
                if (conn!=null)
                {
                    SqlCommand cmd = new SqlCommand("usp_UpdateUser", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@fullName", user.fullName);
                    cmd.Parameters.AddWithValue("@email", user.email);
                    cmd.Parameters.AddWithValue("@password", HashPassword(user.password));
                    cmd.Parameters.AddWithValue("@mobile", user.mobile);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return user;
                }
                else
                {
                    return null;
                }

            }catch(Exception ex) { throw ex; }
            finally { conn.Close(); }
        }



        public bool CheckEmail(string email)
        {
            UserModel model = null;
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_CheckEmail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        model = new UserModel()
                        {
                            fullName = (string)reader["fullname"],
                            email = (string)reader["email"],
                            password = (string)reader["password"],
                            mobile = (long)reader["mobile"]
                        };
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }catch(Exception ex) { throw ex; }
            finally{ conn.Close(); }
        }

       public ForgotPasswordModel ForgotPassword(string email)
        {
            ForgotPasswordModel model = null;
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_ForgotPassword", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@email", email);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        model = new ForgotPasswordModel()
                        {
                            email = (string)reader["email"],
                            userId = (int)(reader["userId"]),
                            Token = GenerateToken(email, (int)(reader["userId"]))
                        }; 
                    }
                    return model;
                }
                else
                
                {
                    return null;
                }

            }catch(Exception ex) { throw ex; }
        }

       public bool ResetPassword(string email, ResetPasswordModel resetModel)
        {
            ResetPasswordModel model = null;
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_ResetPassword", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", HashPassword(resetModel.Password));
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                else
                {
                    return false;
                }
            }catch(Exception ex)
            {
                throw ex;
            }
            finally { conn.Close(); }
        }
    }
}
