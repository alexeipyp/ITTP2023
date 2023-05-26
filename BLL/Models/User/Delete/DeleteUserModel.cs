using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.User.Delete
{
    public class DeleteUserModel : UserManipulationModel
    {
        public Guid UserToDeleteGuid { get; set; }
        public bool IsSoft { get; set; }
    }
}
