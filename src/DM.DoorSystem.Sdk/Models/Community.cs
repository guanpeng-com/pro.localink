using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk.Models
{
    public class Community
    {
        [JsonProperty("agtcode")]
        public string AgtCode { get; set; }
        [JsonProperty("departid")]
        public string DepartID { get; set; }
        [JsonProperty("departname")]
        public string DepartName { get; set; }
        [JsonProperty("departtel")]
        public string DepartTel { get; set; }
        [JsonProperty("departmail")]
        public string DepartMail { get; set; }
        [JsonProperty("street")]
        public string Street { get; set; }
        [JsonProperty("style")]
        public string Style { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("createsysdate")]
        public string CreateSysDate { get; set; }
    }
}
