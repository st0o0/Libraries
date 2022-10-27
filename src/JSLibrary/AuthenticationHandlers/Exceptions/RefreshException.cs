using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSLibrary.AuthenticationHandlers.Exceptions
{
    public class RefreshException : Exception
    {
        public RefreshException() : base()
        {
        }

        public RefreshException(string message) : base(message)
        {
        }
    }
}
