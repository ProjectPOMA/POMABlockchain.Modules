using Newtonsoft.Json;

namespace POMABlockchain.Modules.RPC.DTOs
{
    public class ValidateAddress
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("isvalid")]
        public bool IsValid { get; set; }
    }
}
