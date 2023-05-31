using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Common.Utils;

namespace Common.Extentions
{
    public static class ClaimPrincipalExtention
    {
        public static T? GetClaimValue<T>(this ClaimsPrincipal user, string claim)
        {
            var value = user.Claims.FirstOrDefault(x => x.Type == claim)?.Value;
            if (value != null)
                return Converter.Convert<T>(value);
            else
                return default;
        }
    }
}
