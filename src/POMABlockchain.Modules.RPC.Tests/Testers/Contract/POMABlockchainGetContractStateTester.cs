using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Contract;
using System;
using System.Threading.Tasks;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Contract
{
	public class POMABlockchainGetContractStateTester : RpcRequestTester<ContractState>
	{
		[Fact]
		public async void ShouldReturnContractState()
		{
			var result = await ExecuteAsync();
			Assert.NotNull(result);
		}

		public override async Task<ContractState> ExecuteAsync(IClient client)
		{
			var contractState = new POMABlockchainGetContractState(client);
			return await contractState.SendRequestAsync(Settings.GetContractHash());
		}

		public override Type GetRequestType()
		{
			return typeof(ContractState);
		}
	}
}
