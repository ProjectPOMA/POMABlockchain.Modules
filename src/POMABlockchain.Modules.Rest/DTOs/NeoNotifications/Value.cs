using Newtonsoft.Json;

namespace POMABlockchain.Modules.Rest.DTOs.POMABlockchainNotifications
{
    public class Value
    {
        [JsonProperty("type")]
        public StateType Type { get; set; }

        [JsonProperty("value")]
        public string ValueValue { get; set; }
    }
}
