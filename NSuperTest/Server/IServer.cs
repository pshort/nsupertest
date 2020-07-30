using NSuperTest.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace NSuperTest.Server
{
    public interface IServer
    {
        string Address { get; }
        bool UseCamelCase { get; }

        IHttpRequestClient GetClient();
    }
}
