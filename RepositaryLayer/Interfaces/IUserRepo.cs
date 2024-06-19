using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositaryLayer.Interfaces
{
    public interface IUserRepo
    {
        List<UserModel> FetchAllUsers();
        UserModel FetchByUSerId(int userId);
        string LoginUser(string email, string password);
        UserModel RegisterUser(UserModel user);
        UserModel UpdateUser(int userId, UserModel user);

        public bool CheckEmail(string email);
        ForgotPasswordModel ForgotPassword(string email);
        bool ResetPassword(string email, ResetPasswordModel resetModel);
    }
}
