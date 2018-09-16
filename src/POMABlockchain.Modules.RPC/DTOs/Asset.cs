using System.Collections.Generic;
using Newtonsoft.Json;

namespace POMABlockchain.Modules.RPC.DTOs
{
    public class Asset
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public List<Name> Name { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("precision")]
        public int Precision { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("admin")]
        public string Admin { get; set; }
    }
}
