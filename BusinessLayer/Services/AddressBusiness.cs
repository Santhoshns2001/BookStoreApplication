using BusinessLayer.Interfaces;
using ModelLayer;
using RepositaryLayer.Entities;
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

        public Addresses AddAddress(int userId, AddressModel addressModel)
        {
            return addressRepo.AddAddress(userId, addressModel);
        }

       public List<Addresses> GetAddressesByUser(int userid)
        {
           return addressRepo.GetAddressesByUser(userid);
        }

       public List<Addresses> GetAddressesByUserIdAndAddressId(int userid, int addressId)
        {
           return addressRepo.GetAddressesByUserIdAndAddressId(userid, addressId);
        }

      public  List<Addresses> GetAllAddresses()
        {
            return addressRepo.GetAllAddresses();   
        }

      public bool RemoveAddress(int userid, int addressId)
        {
           return addressRepo.RemoveAddress(userid, addressId);
        }

      public Addresses UpdateAddress(int userid, int addressid, AddressModel model)
        {
          return addressRepo.UpdateAddress(userid,addressid,model);
        }
    }
}
