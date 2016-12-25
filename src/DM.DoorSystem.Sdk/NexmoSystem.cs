using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk
{
    public class NexmoSystem : ISdk
    {
        public NexmoSystem() { }

        public string Version
        {
            get
            {
                return "1.0.0";
            }
        }

        public string ApiBase
        {
            get
            {
                return "https://rest.nexmo.com";
            }
        }

        public string TestApiBase
        {
            get
            {
                return "https://rest.nexmo.com";
            }
        }

        public bool IsTest
        {
            get
            {
                return true;
            }
        }

        public int DefaultTimeout
        {
            get
            {
                return 1200000;
            }
        }

        public int DefaultReadAndWriteTimeout
        {
            get
            {
                return 1200000;
            }
        }

        public Dictionary<string, string> Params
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("api_key", "62523f9e");
                dic.Add("api_secret", "a62f59dd6eb84ed6");
                return dic;
            }
        }

        public string GetApiBaseUrl()
        {
            if (IsTest)
                return TestApiBase;
            else
                return ApiBase;
        }
    }
}
