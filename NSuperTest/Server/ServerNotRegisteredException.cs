using System;
using System.Collections.Generic;
using System.Text;

namespace NSuperTest.Server
{
    public class ServerNotRegisteredException : Exception
    {
        public ServerNotRegisteredException(string message)
            : base(message)
        {

        }
    }
}
