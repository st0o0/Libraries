using System;

namespace JSLibrary.AuthenticationHandlers.Exceptions
{
    public class LoginException : Exception
    {
        public LoginException() : base()
        {
        }

        public LoginException(string message) : base(message)
        {
        }
    }
}