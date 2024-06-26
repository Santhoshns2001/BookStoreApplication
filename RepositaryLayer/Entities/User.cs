using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositaryLayer.Entities
{
    public class User
    {
        public int userId { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public long mobile { get; set; }

    }
}
