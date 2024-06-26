using ModelLayer;
using RepositaryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IUserBuss
    {
        List<User> FetchAllUsers();
        User FetchByUSerId(int userId);
        string LoginUser(string email, string password);
        User RegisterUser (UserModel user);
        User UpdateUser(int userId, UserModel user);
        public bool CheckEmail(string email);
        ForgotPasswordModel ForgotPassword(string email);
        bool ResetPassword(string email, ResetPasswordModel resetModel);
    }
}
