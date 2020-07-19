using System;

namespace NSuperTest.Server
{
    public class ServerCreationException : Exception
    {
        public ServerCreationException(string message) : base(message) {}
    }
}