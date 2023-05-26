using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.User.Update
{
    public class UpdateUserLoginModel : UpdateUserModel
    {
        public string Login { get; set; } = null!;
    }
}
