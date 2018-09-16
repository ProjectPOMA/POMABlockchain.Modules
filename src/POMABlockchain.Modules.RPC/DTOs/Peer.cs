using Newtonsoft.Json;

namespace POMABlockchain.Modules.RPC.DTOs
{
    public class Peer
    {
		[JsonProperty("address")]
		public string Address { get; set; }

		[JsonProperty("port")]
		public string Port { get; set; }
	}
}
