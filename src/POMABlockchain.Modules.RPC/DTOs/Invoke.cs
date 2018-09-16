using Newtonsoft.Json;
using System.Collections.Generic;

namespace POMABlockchain.Modules.RPC.DTOs
{
	public class Invoke
	{
        [JsonProperty("script")]
        public string Script { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("gas_consumed")]
		public string GasConsumed { get; set; }

		[JsonProperty("stack")]
		public List<Stack> Stack { get; set; }
	}
}
