using Newtonsoft.Json;

namespace POMABlockchain.Modules.RPC.DTOs
{
	public class InvokeParameter
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }
	}
}
