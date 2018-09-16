using Newtonsoft.Json;
using System.Collections.Generic;

namespace POMABlockchain.Modules.RPC.DTOs
{
    public class Peers
    {
		[JsonProperty("bad")]
		public List<Peer> Bad { get; set; }

		[JsonProperty("connected")]
		public List<Peer> Connected { get; set; }

		[JsonProperty("unconnected")]
		public List<Peer> Unconnected { get; set; }
	}
}
