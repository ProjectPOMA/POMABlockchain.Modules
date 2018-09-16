using Newtonsoft.Json;

namespace POMABlockchain.Modules.RPC.DTOs
{
	public class SendManyParameter
	{
		[JsonProperty("asset")]
		public string Asset { get; set; }

		[JsonProperty("value")]
		public double Value { get; set; }

		[JsonProperty("address")]
		public string Address { get; set; }
	}
}
