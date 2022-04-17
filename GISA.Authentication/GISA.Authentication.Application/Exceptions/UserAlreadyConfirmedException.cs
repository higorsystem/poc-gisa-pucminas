using System;

namespace GISA.Authentication.Application.Exceptions
{
    public class UserAlreadyConfirmedException : Exception
    {
        public UserAlreadyConfirmedException(string message) : base(message)
        {

        }
    }
}