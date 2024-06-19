using BusinessLayer.Interfaces;
using ModelLayer;
using RepositaryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserBusiness : IUserBuss
    {
        private readonly IUserRepo userRepo;
        public UserBusiness(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public UserModel RegisterUser(UserModel user)
        {
            return userRepo.RegisterUser(user);

            
        }

        public string LoginUser(string email, string password)
        {
            return userRepo.LoginUser(email,password);
        }

       public UserModel FetchByUSerId(int userId)
        {
           return userRepo.FetchByUSerId(userId);
        }

        public List<UserModel> FetchAllUsers()
        {
            return userRepo.FetchAllUsers();
        }

       public UserModel UpdateUser(int userId, UserModel user)
        {
           return userRepo.UpdateUser(userId, user);
        }

       public  bool CheckEmail(string email)
        {
           return userRepo.CheckEmail(email);
        }

        public ForgotPasswordModel ForgotPassword(string email)
        {
            return userRepo.ForgotPassword(email);
        }

       public bool ResetPassword(string email, ResetPasswordModel resetModel)
        {
            return userRepo.ResetPassword(email,resetModel);
        }
    }
}
