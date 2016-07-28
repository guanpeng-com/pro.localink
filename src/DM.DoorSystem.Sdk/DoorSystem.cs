using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk
{
    public abstract class DoorSystem
    {
        public static volatile string Version = "1.0.0";
        public static volatile string ApiBase = "http://121.40.204.191:8180/mdserver/service";
        public static volatile string TestApiBase = "http://121.40.204.191:8180/mdserver/service";
        public static volatile bool IsTest = true;
        public static int DefaultTimeout = 80000;
        public static int DefaultReadAndWriteTimeout = 20000;

        public static string GetApiBaseUri()
        {
            if (IsTest)
                return TestApiBase;
            else
                return ApiBase;
        }

        /// <summary>
        /// 服务端认证key
        /// </summary>
        public static volatile string AppKey = "24e504a6efee47fe8dadfc0a3afd3d46";

        /// <summary>
        /// 服务端认证编号
        /// </summary>
        public static volatile string AgtNum = "80202";

        public static void SetAppKey(string appKey)
        {
            AppKey = appKey;
        }

        public static string GetAppKey()
        {
            return AppKey;
        }

        public static void SetAgeNum(string agtNum)
        {
            AgtNum = agtNum;
        }

        public static string GetAgeNum()
        {
            return AgtNum;
        }
    }
}
