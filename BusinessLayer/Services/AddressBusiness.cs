using BusinessLayer.Interfaces;
using RepositaryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class AddressBusiness :IAddressBuss
    {
        private readonly IAddressRepo addressRepo;

        public AddressBusiness(IAddressRepo addressRepo)
        {
            this.addressRepo = addressRepo;
        }

    }
}
