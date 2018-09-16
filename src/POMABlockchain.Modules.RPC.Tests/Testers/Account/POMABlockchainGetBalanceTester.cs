using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Account;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Account
{
    public class POMABlockchainGetBalanceTester : RpcRequestTester<WalletBalance>
    {
        [Fact]
        public async void ShouldReturnWalletBalance()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<WalletBalance> ExecuteAsync(IClient client)
        {
            var balance = new POMABlockchainGetBalance(client);
            return await balance.SendRequestAsync(Settings.GetGoverningAssetHash());
        }

        public override Type GetRequestType()
        {
            return typeof(WalletBalance);
        }
    }
}
