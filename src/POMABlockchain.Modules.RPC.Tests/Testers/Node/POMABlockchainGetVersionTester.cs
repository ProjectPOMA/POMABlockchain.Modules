using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Node;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Node
{
    public class POMABlockchainGetVersionTester : RpcRequestTester<Version>
	{
		[Fact]
		public async void ShouldReturnVersion()
		{
			var result = await ExecuteAsync();
			Assert.NotNull(result);
		}

		public override async Task<Version> ExecuteAsync(IClient client)
		{
			var version = new POMABlockchainGetVersion(client);
			return await version.SendRequestAsync();
		}

		public override System.Type GetRequestType()
		{
			return typeof(Version);
		}
	}
}
