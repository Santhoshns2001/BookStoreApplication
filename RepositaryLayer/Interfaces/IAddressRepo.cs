using ModelLayer;
using RepositaryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositaryLayer.Interfaces
{
    public interface IAddressRepo
    {
        public Addresses AddAddress(int userId, AddressModel addressModel);
        public List<Addresses> GetAllAddresses();
        public List<Addresses> GetAddressesByUser(int userid);
        public List<Addresses> GetAddressesByUserIdAndAddressId(int userid, int addressId);
        public Addresses UpdateAddress(int userid, int addressid, AddressModel model);
        public bool RemoveAddress(int userid, int addressId);
    }
}
