using Newtonsoft.Json;

namespace POMABlockchain.Modules.Rest.DTOs.POMABlockchainScan
{
    public class Script
    {
        [JsonProperty("verification")]
        public string Verification { get; set; }

        [JsonProperty("invocation")]
        public string Invocation { get; set; }
    }
}
