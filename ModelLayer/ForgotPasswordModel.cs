using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class ForgotPasswordModel
    {
        public int userId { get; set; }

        public string email { get; set; }

        public string Token { get; set; }
    }
}
