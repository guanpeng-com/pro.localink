using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk
{
    public interface ISdk
    {
        string Version { get; }
        string ApiBase { get; }
        string TestApiBase { get; }
        bool IsTest { get; }
        int DefaultTimeout { get; }
        int DefaultReadAndWriteTimeout { get; }

        string GetApiBaseUrl();

        Dictionary<string, string> Params { get; }
    }
}
