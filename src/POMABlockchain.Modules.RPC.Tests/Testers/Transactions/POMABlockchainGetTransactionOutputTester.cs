using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Transactions;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Transactions
{
    public class POMABlockchainGetTransactionOutputTester : RpcRequestTester<TransactionOutput>
    {
        [Fact]
        public async void ShouldReturnTransactionOutput()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<TransactionOutput> ExecuteAsync(IClient client)
        {
            var transactionOutput = new POMABlockchainGetTransactionOutput(client);
            return await transactionOutput.SendRequestAsync(
                "7f7f3b361e46b271e15c640d40994f759ce13f608ac53fd970b9d6db779dd589");
        }

        public override Type GetRequestType()
        {
            return typeof(TransactionOutput);
        }
    }
}