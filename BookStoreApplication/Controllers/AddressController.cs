using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    }
}
