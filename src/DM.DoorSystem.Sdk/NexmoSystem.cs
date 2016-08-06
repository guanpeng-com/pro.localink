﻿using System;
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
                dic.Add("api_key", "249bc9a0");
                dic.Add("api_secret", "d4e1a410c8e9bdae");
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
