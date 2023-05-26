using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.User.Update
{
    public class UpdateUserModel : UserManipulationModel
    {
        public Guid UserToUpdateGuid { get; set; }
        public string ModifiedBy { get; set; } = null!;
    }
}
