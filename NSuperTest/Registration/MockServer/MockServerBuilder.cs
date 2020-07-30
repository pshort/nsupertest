using NSuperTest.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace NSuperTest.Registration.MockServer
{
    public class MockServerBuilder : IServerBuilder
    {
        private string _address;

        public MockServerBuilder(string address)
        {
            _address = address;
        }

        public IServer Build()
        {
            // this is a transitive strategy -> new each time
            return new MockServer(_address, true);
        }
    }
}
