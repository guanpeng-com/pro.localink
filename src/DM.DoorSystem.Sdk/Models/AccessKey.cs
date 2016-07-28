using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk.Models
{
    public class AccessKey
    {
        [JsonProperty("agtcode")]
        public string AgtCode { get; set; }
        [JsonProperty("app_key")]
        public string AppKye { get; set; }
        [JsonProperty("algorithmVer")]
        public string AlgorithmVer { get; set; }
        [JsonProperty("user_id")]
        public string UserID { get; set; }
        [JsonProperty("lock_name")]
        public string LockName { get; set; }
        [JsonProperty("community")]
        public string Community { get; set; }
        [JsonProperty("lock_id")]
        public string LockID { get; set; }
        [JsonProperty("pid")]
        public string PID { get; set; }
    }
}
