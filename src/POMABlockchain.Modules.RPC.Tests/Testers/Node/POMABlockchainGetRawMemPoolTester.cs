using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Node;
using System;
using System.Threading.Tasks;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Node
{
	public class POMABlockchainGetRawMemPoolTester : RpcRequestTester<string[]>
	{
		[Fact]
		public async void ShouldReturnMemoryPool()
		{
			var result = await ExecuteAsync();
			Assert.NotNull(result);
			if(result != null) Assert.All(result, str => str.StartsWith("0x"));
		}

		public override async Task<string[]> ExecuteAsync(IClient client)
		{
			var memoryPool = new POMABlockchainGetRawMemPool(client);
			return await memoryPool.SendRequestAsync();
		}

		public override Type GetRequestType()
		{
			return typeof(string[]);
		}
	}
}
