using System.Collections.Generic;
using Newtonsoft.Json;

namespace POMABlockchain.Modules.Rest.DTOs.POMABlockchainScan
{
    public class Node
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        public static IList<Node> FromJson(string json) => JsonConvert.DeserializeObject<IList<Node>>(json, Utils.Settings);
    }
}