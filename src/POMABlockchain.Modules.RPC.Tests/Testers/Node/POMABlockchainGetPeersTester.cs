using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Node;
using System;
using System.Threading.Tasks;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Node
{
	public class POMABlockchainGetPeersTester : RpcRequestTester<Peers>
	{
		[Fact]
		public async void ShouldReturnPeers()
		{
			var result = await ExecuteAsync();
			Assert.NotNull(result);
		}

		public override async Task<Peers> ExecuteAsync(IClient client)
		{
			var peers = new POMABlockchainGetPeers(client);
			return await peers.SendRequestAsync();
		}

		public override Type GetRequestType()
		{
			return typeof(Peers);
		}
	}
}
