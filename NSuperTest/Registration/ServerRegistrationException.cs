using System;
using System.Collections.Generic;
using System.Text;

namespace NSuperTest.Registration
{
    public class ServerRegistrationException : Exception
    {
        public ServerRegistrationException(string message)
            : base(message)
        {

        }
    }
}
