using BusinessLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;

namespace BookStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBuss userBuss;
        private readonly IBus bus;

        public UserController(IUserBuss userBuss, IBus bus)
        {
            this.userBuss = userBuss;
            this.bus = bus;
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult RegisterUser(UserModel userModel)
        {
            var response = userBuss.RegisterUser(userModel);

            if (response != null)
            {
                return Ok(new ResponseModel<UserModel> { IsSuccuss = true, Message = "process succuss", Data = response });
            }
            else
            {
                return BadRequest(new ResponseModel<UserModel> { IsSuccuss = false, Message = " failed to insert", Data = response });
            }

        }



        [HttpPost]
        [Route("Login")]
        public ActionResult LoginUser (string email,string password)
        {
            var response = userBuss.LoginUser(email, password);
            if (response != null)
            {
                return Ok(new ResponseModel<string> { IsSuccuss = true, Message = "login succuss", Data = response });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to login", Data = response });
            }
        }


        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {

            if (userBuss.CheckEmail(email))
            {
                Send send = new Send();
                ForgotPasswordModel forgotPasswordModel = userBuss.ForgotPassword(email);
                send.SendMail(forgotPasswordModel.email, forgotPasswordModel.Token);
                Uri uri = new Uri("rabbitmq://localhost/BookStoreUserEmailQueue");

                var endPoint = await bus.GetSendEndpoint(uri);
                await endPoint.Send(forgotPasswordModel);


                return Ok(new ResponseModel<ForgotPasswordModel>() { IsSuccuss = true, Message = "mail sent succussfully", Data = forgotPasswordModel });
            }
            else
            {
                return BadRequest(new ResponseModel<string>() { IsSuccuss = false, Message = "Mail Not sent", Data = "not succuss" });
            }

        }

        [Authorize]
        [HttpPost]
        [Route("ResetPassword")]
        public ActionResult ResetPassword(ResetPasswordModel resetModel) 
        {
            try
            {
                if (resetModel.Password == resetModel.ConfirmPassword)
                {
                    string Email = User.FindFirst("Email").Value;
                    if (userBuss.ResetPassword(Email, resetModel))
                    {
                        return Ok(new ResponseModel<string> { IsSuccuss = true, Message = "password reset succussfull", Data = "password matched and succussfully changed " });
                    }
                    else
                    {
                        return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = "password reset unsuccessfull ", Data = "password unmtched please check the password " });
                    }
                }
                else
                {
                    return BadRequest(new ResponseModel<string>() { IsSuccuss = false, Message = "password and confirm password does not match", Data = "please check the password and confirm password " });
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        [Route("FetchByUserId")]
        public ActionResult FetchByUserId(int userId)
        {   
            UserModel result=userBuss.FetchByUSerId(userId);
            if (result != null)
            {
                return Ok(new ResponseModel<UserModel> { IsSuccuss = true, Message = "user fetched succussfully", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = "wroong  id is provided " });
            }

        }

        [HttpGet]
        [Route("FetchAllUsers")]
        public ActionResult FetchAllUsers()
        {
            List<UserModel> users = userBuss.FetchAllUsers();

            if (users != null)
            {
                return Ok(new ResponseModel<List<UserModel>> { IsSuccuss = true, Message = "list of users fetched succussfully", Data = users });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = "something went wrong" });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateUser")]
        public ActionResult UpdateUser( UserModel user) 
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            UserModel userModel=userBuss.UpdateUser(userId, user);
            if (userModel != null)
            {
                return Ok(new ResponseModel<UserModel>{ IsSuccuss = true, Message = "updated succussfully", Data = userModel });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to update", Data = "something went wrong" });
            }
        }


    }
}
