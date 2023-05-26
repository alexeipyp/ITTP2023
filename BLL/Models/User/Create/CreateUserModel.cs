using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.User.Create
{
    public class CreateUserModel : UserManipulationModel
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Gender { get; set; } = (int)Common.Enums.Gender.Unknown;
        public DateTime? Birthday { get; set; }
        public bool Admin { get; set; }
        public string CreatedBy { get; set; } = null!;
    }
}
