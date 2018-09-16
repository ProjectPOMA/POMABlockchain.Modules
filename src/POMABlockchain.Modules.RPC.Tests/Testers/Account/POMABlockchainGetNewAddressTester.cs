using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Account;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Account
{
    public class POMABlockchainGetNewAddressTester : RpcRequestTester<string>
    {
        [Fact]
        public async void ShouldReturnWalletBalance()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<string> ExecuteAsync(IClient client)
        {
            var newAddress = new POMABlockchainGetNewAddress(client);
            return await newAddress.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(string);
        }
    }
}
