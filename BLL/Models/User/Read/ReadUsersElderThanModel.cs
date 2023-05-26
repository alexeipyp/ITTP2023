using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.User.Read
{
    public class ReadUsersElderThanModel : UserManipulationModel
    {
        public int Age { get; set; }
    }
}
