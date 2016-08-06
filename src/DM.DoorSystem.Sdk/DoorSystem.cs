using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk
{
    public class DoorSystem : ISdk
    {
        public DoorSystem() { }

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
                return "http://121.40.204.191:8180/mdserver/service";
            }
        }

        public string TestApiBase
        {
            get
            {
                return "http://121.40.204.191:8180/mdserver/service";
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
                return 80000;
            }
        }

        public int DefaultReadAndWriteTimeout
        {
            get
            {
                return 80000;
            }
        }

        public Dictionary<string, string> Params
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("app_key", "24e504a6efee47fe8dadfc0a3afd3d46");
                dic.Add("agt_num", "80202");
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
