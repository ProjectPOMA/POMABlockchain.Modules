﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace POMABlockchain.Modules.Rest.DTOs.POMABlockchainScan
{
    public class Claimed
    {
        [JsonProperty("claimed")]
        public IList<ClaimedElement> ClaimedList { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        public static Claimed FromJson(string json) => JsonConvert.DeserializeObject<Claimed>(json, Utils.Settings);
    }
}
