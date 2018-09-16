using Newtonsoft.Json;

namespace POMABlockchain.Modules.Rest.DTOs.POMABlockchainScan
{
    public class Unclaimed
    {
        [JsonConstructor]
        public Unclaimed(float unclaimedValue, string address)
        {
            UnclaimedValue = unclaimedValue;
            Address = address;
        }

        [JsonProperty("unclaimed")]
        public float UnclaimedValue { get; set; }

        public string Address { get; set; }

        public static Unclaimed FromJson(string json) => JsonConvert.DeserializeObject<Unclaimed>(json, Utils.Settings);
    }
}