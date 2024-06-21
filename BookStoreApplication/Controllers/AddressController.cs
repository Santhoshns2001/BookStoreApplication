using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositaryLayer.Entities;

namespace BookStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressBuss addressBuss;

        public AddressController(IAddressBuss addressBuss)
        {
            this.addressBuss = addressBuss;
        }

        [Authorize]
        [HttpPost]
        [Route("AddAddress")]
        public IActionResult AddAddress( AddressModel addressModel)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            Addresses address = addressBuss.AddAddress(userId,addressModel);

            if (address != null)
            {
                return Ok(new ResponseModel<Addresses> { IsSuccuss = true, Message = "address added succussfully", Data = address });
            }
            else
            {
                return BadRequest(new ResponseModel<Addresses> { IsSuccuss = false, Message = " failed to add book", Data = address });
            }

        }
        [Authorize]
        [HttpGet]
        [Route("GetAllAddresses")]
        public IActionResult GetAllAddresses()
        {
            List<Addresses> addresses = addressBuss.GetAllAddresses();

            if (addresses != null)
            {
                return Ok(new ResponseModel<List<Addresses>> { IsSuccuss = true, Message = "list of addresses fetched succussfully", Data = addresses });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = "something went wrong" });
            }
        }

        [Authorize]
        [HttpGet("GetAddressByUserId")]
        public IActionResult GetAddressesByUser()
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            var addresses = addressBuss.GetAddressesByUser(userId);
            if (addresses != null)
            {
                return Ok(new ResponseModel<List<Addresses>> { IsSuccuss = true, Message = "address fetched succussfully", Data = addresses });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch address", Data = "wrong address id is been provided " });
            }
        }
        [Authorize]
        [HttpPut("UpdateAddress")]
        public IActionResult UpdateAddress(int addressId, AddressModel model)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            Addresses address = addressBuss.UpdateAddress(userId, addressId,model);
            if (address != null)
            {
                return Ok(new ResponseModel<Addresses> { IsSuccuss = true, Message = "address updated  succussfully", Data = address });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to update", Data = "wrong input is been provided " });
            }

        }

        [HttpGet("GetAddressesByUserIdAndAddressId")]
        public IActionResult GetAddressesByUserIdAndAddressId( int addressid)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            List<Addresses> addresses = addressBuss.GetAddressesByUserIdAndAddressId(userId, addressid);
            if (addresses != null)
            {
                return Ok(new ResponseModel<List<Addresses>> { IsSuccuss = true, Message = "addresses fetched  succussfully", Data = addresses });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = " no matched input" });
            }

        }
        [Authorize]
        [HttpDelete("RemoveAddress")]
        public IActionResult RemoveAddress(int addressid)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            bool res = addressBuss.RemoveAddress(userId,addressid);
            if (res)
            {
                return Ok(new ResponseModel<string> { IsSuccuss = true, Message = "address is been deleted", Data = "deleted succussfully" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to delete", Data = "wrong input is been provided " });
            }

        }
    }
}
