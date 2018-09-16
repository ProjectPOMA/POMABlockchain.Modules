using Newtonsoft.Json;

namespace POMABlockchain.Modules.RPC.DTOs
{
	public class Stack
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("value")]
		public object Value { get; set; }
	}
}
