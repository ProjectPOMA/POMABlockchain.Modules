using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Contract;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Contract
{
	public class POMABlockchainGetStorageTester : RpcRequestTester<string> //todo
	{
		[Fact]
		public async void ShouldReturnStorage()
		{
			var result = await ExecuteAsync();
			Assert.Null(result);
		}

		public override async Task<string> ExecuteAsync(IClient client)
		{
			var contractState = new POMABlockchainGetStorage(client);
			return await contractState.SendRequestAsync("03febccf81ac85e3d795bc5cbd4e84e907812aa3", "5065746572");
		}

		public override Type GetRequestType()
		{
			return typeof(string);
		}
	}
}
