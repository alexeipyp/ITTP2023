using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.User.Read
{
    public class ReadUserByLoginModel : UserManipulationModel
    {
        public string UserToReadLogin { get; set; } = null!;
    }
}
