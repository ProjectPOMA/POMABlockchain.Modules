﻿using Newtonsoft.Json;

namespace POMABlockchain.Modules.Rest.DTOs.POMABlockchainScan
{
    public class AbstractEntry
    {
        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("block_height")]
        public long BlockHeight { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("address_to")]
        public string AddressTo { get; set; }

        [JsonProperty("address_from")]
        public string AddressFrom { get; set; }
    }
}
