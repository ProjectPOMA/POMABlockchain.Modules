﻿using Newtonsoft.Json;

namespace POMABlockchain.Modules.RPC.DTOs
{
    public class Validator
    {
        [JsonProperty("publickey")]
        public string PublicKey { get; set; }

        [JsonProperty("votes")]
        public string Votes { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
