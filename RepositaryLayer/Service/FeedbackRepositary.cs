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
    public class FeedbackRepositary:IFeedbackRepo
    {
        private readonly SqlConnection conn = new SqlConnection();

        private readonly string sqlConnectionString;

        private readonly IConfiguration configuration;

        public FeedbackRepositary(IConfiguration configuration)
        {
            this.configuration = configuration;
            sqlConnectionString = configuration.GetConnectionString("DbConnection");
            conn.ConnectionString = sqlConnectionString;
        }

        public Feedback AddFeedBack( int userid,FeedbackModel feedbackModel)
        {
            Feedback feedback = null;
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_AddFeedback", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userid);
                    cmd.Parameters.AddWithValue("@BookId", feedbackModel.BookId);
                    cmd.Parameters.AddWithValue("@Rating", feedbackModel.Rating);
                    cmd.Parameters.AddWithValue("@Review", feedbackModel.Review);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        feedback = new Feedback()
                        {
                            FeedbackId = (int)reader["FeedbackId"],
                            UserId = (int)reader["UserId"],
                            UserName=(string)reader["UserName"],
                            BookId = (int)reader["BookId"],
                            Rating = (int)reader["Rating"],
                            Review = (string)reader["Review"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            UpdatedAt = (DateTime)reader["UpdatedAt"]

                        };
                        return feedback;
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

        public Feedback EditFeedback(int userid, EditFeedbackModel editFeedbackModel)
        {
            Feedback feedback = null;
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_UpdateFeedback", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@FeedbackId", editFeedbackModel.FeedbackId);
                    cmd.Parameters.AddWithValue("@UserId", userid);
                    cmd.Parameters.AddWithValue("@Review", editFeedbackModel.Review);
                    cmd.Parameters.AddWithValue("@Rating", editFeedbackModel.Rating);
                    
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        feedback = new Feedback()
                        {
                            FeedbackId = (int)reader["FeedbackId"],
                            UserId = (int)reader["UserId"],
                            UserName = (string)reader["UserName"],
                            BookId = (int)reader["BookId"],
                            Rating = (int)reader["Rating"],
                            Review = (string)reader["Review"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            UpdatedAt = (DateTime)reader["UpdatedAt"]
                        };
                        return feedback;
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

        public bool RemoveFeedback(int feedbackid,int userid)
        {
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_RemoveFeedback", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FeedbackId", feedbackid);
                    cmd.Parameters.AddWithValue("@UserId", userid);
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
        public List<Feedback> ViewAllFeedbacks()
        {
            List<Feedback> feedbacks = new List<Feedback>();
            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_ViewAllFeedbacks", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Feedback feedback = new Feedback()
                        {
                            FeedbackId = (int)reader["FeedbackId"],
                            UserId = (int)reader["UserId"],
                            UserName = (string)reader["UserName"],
                            BookId = (int)reader["BookId"],
                            Rating = (int)reader["Rating"],
                            Review = (string)reader["Review"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            UpdatedAt = (DateTime)reader["UpdatedAt"]
                        };
                        feedbacks.Add(feedback);
                    }
                    return feedbacks;
                }
                else
                {
                    throw new Exception("connection was not established");
                }
            }
            catch (Exception ex) { throw ex; }
            finally { conn.Close(); }
        }

        public List<Feedback> ViewFeedbackByBookId(int bookid)
        {
            List<Feedback> feedbacks = new List<Feedback>();

            try
            {
                if (conn != null)
                {
                    SqlCommand cmd = new SqlCommand("usp_ViewFeedbacksByBookId", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", bookid);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Feedback feedback = new Feedback()
                        {
                            FeedbackId = (int)reader["FeedbackId"],
                            UserId = (int)reader["UserId"],
                            UserName = (string)reader["UserName"],
                            BookId = (int)reader["BookId"],
                            Rating = (int)reader["Rating"],
                            Review = (string)reader["Review"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            UpdatedAt = (DateTime)reader["UpdatedAt"]
                        };
                        feedbacks.Add(feedback);
                    }
                    return feedbacks;
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




    }
}
