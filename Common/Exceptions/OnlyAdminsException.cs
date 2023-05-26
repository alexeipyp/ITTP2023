using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class OnlyAdminsException : Exception
    {
        public override string Message => "Недостаточно полномочий для выполнения запроса";
        public OnlyAdminsException()
        {
        }

        public OnlyAdminsException(string message)
            : base(message)
        {
        }

        public OnlyAdminsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
