using System.Collections.Generic;
using Newtonsoft.Json;

namespace POMABlockchain.Modules.Rest.DTOs.POMABlockchainScan
{
    public class ClaimedElement
    {
        [JsonProperty("txids")]
        public IList<string> Txids { get; set; }
    }
}
