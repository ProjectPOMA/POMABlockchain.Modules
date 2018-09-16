using System;
using System.Threading.Tasks;
using Xunit;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Account;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Account
{
    public class POMABlockchainListAddressesTester : RpcRequestTester<WalletAddress[]>
    {
        [Fact]
        public async void ShouldReturnWalletBalance()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<WalletAddress[]> ExecuteAsync(IClient client)
        {
            var listAddresses = new POMABlockchainListAddresses(client);
            return await listAddresses.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(WalletAddress[]);
        }
    }
}

