using NSuperTest.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace NSuperTest.Registration
{
    public interface IServerBuilder
    {
        IServer Build();
    }
}
