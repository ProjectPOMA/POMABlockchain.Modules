﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace POMABlockchain.Modules.Rest.DTOs.POMABlockchainScan
{
    public class Block
    {
        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("tx_count")]
        public long TxCount { get; set; }

        [JsonProperty("transfers")]
        public IList<string> Transfers { get; set; }

        [JsonProperty("transactions")]
        public IList<string> Transactions { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("script")]
        public Script Script { get; set; }

        [JsonProperty("previousblockhash")]
        public string Previousblockhash { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("nextconsensus")]
        public string Nextconsensus { get; set; }

        [JsonProperty("nextblockhash")]
        public string Nextblockhash { get; set; }

        [JsonProperty("merkleroot")]
        public string Merkleroot { get; set; }

        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("confirmations")]
        public long Confirmations { get; set; }

        public static Block FromJson(string json) => JsonConvert.DeserializeObject<Block>(json, Utils.Settings);
    }

    public class Blocks
    {
        public static IList<Block> FromJson(string json) => JsonConvert.DeserializeObject<IList<Block>>(json, Utils.Settings);
    }
}
