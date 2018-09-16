using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Transactions;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Transactions
{
    public class POMABlockchainSendToAddressTester : RpcRequestTester<DTOs.Transaction> // todo: add a way to test method that need an open wallet
    {
        [Fact]
        public async void ShouldReturnWalletBalance()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<DTOs.Transaction> ExecuteAsync(IClient client)
        {
            var sendAssets = new POMABlockchainSendToAddress(client);
            return await sendAssets.SendRequestAsync(Settings.GetGoverningAssetHash(), Settings.GetAddress(), 1);
        }

        public override Type GetRequestType()
        {
            return typeof(DTOs.Transaction);
        }
    }
}
