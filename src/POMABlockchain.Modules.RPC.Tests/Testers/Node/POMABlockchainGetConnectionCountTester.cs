using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Node;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Node
{
    public class POMABlockchainGetConnectionCountTester : RpcRequestTester<int>
	{
		[Fact]
		public async void ShouldReturnConnectionCount()
		{
			var result = await ExecuteAsync();
			Assert.True(result >= 0);
		}

		public override async Task<int> ExecuteAsync(IClient client)
		{
			var connectionCount = new POMABlockchainGetConnectionCount(client);
			return await connectionCount.SendRequestAsync();
		}

		public override Type GetRequestType()
		{
			return typeof(int);
		}
	}
}
