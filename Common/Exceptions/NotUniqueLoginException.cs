using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class NotUniqueLoginException : Exception
    {
        public override string Message => "Пользователь с таким логином уже существует";
        public NotUniqueLoginException()
        {
        }

        public NotUniqueLoginException(string message)
            : base(message)
        {
        }

        public NotUniqueLoginException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
