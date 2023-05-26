using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.User.Update
{
    public class UpdateUserInfoModel : UpdateUserModel
    {
        public string? Name { get; set; }
        public DateTime? Birthday { get; set; }
        public int Gender { get; set; } = (int)Common.Enums.Gender.Unknown;
    }
}
