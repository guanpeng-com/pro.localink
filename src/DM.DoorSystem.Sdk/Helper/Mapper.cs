using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Linq;
using DM.DoorSystem.Sdk.Exception;

namespace DM.DoorSystem.Sdk
{
    public static class Mapper<T>
    {
        public static List<T> MapCollectionFromJson(string json, string data = "data")
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new DSException("No json received");
            }
            var jObject = JObject.Parse(json);
            var allTokens = jObject.SelectToken(data);

            return allTokens.Select(tkn => MapFromJson(tkn.ToString())).ToList();
        }

        public static T MapFromJson(string json, string parentToken = null)
        {
            var jsonToParse = string.IsNullOrEmpty(parentToken) ? json : JObject.Parse(json).SelectToken(parentToken).ToString();

            return JsonConvert.DeserializeObject<T>(jsonToParse);
        }
    }
}
