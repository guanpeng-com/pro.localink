using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk.Models
{
    public class Door
    {
        [JsonProperty("departid")]
        public string DepartID { get; set; }
        [JsonProperty("install_address")]
        public string Address { get; set; }
        [JsonProperty("install_lock_name")]
        public string LockName { get; set; }
    }
}
