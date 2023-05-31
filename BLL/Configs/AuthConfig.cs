using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Configs
{
    public class AuthConfig
    {
        public string Issuer { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public int LifeTime { get; set; }

        public SymmetricSecurityKey SymmetricSecuriryKey()
            => new(Encoding.UTF8.GetBytes(Key));
    }
}
