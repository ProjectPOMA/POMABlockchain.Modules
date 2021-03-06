using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Transactions;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Transactions
{
	public class POMABlockchainGetRawTransactionTester : RpcRequestTester<Transaction>
	{
		[Fact]
		public async void ShouldReturnRawTransaction()
		{
			var result = await ExecuteAsync();
			Assert.NotNull(result);
		}

		public override async Task<Transaction> ExecuteAsync(IClient client)
		{
			var rawTransaction = new POMABlockchainGetRawTransaction(client);
			return await rawTransaction.SendRequestAsync("f4250dab094c38d8265acc15c366dc508d2e14bf5699e12d9df26577ed74d657"); // todo move to settings
		}

		public override Type GetRequestType()
		{
			return typeof(Transaction);
		}
	}
}
