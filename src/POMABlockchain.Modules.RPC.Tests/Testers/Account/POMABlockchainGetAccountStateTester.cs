using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Account;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Account
{
    public class POMABlockchainGetAccountStateTester : RpcRequestTester<AccountState>
    {
        [Fact]
        public async void ShouldReturnAccountState()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<AccountState> ExecuteAsync(IClient client)
        {
            var accountState = new POMABlockchainGetAccountState(client);
            return await accountState.SendRequestAsync(Settings.GetDefaultAccount());
        }

        public override Type GetRequestType()
        {
            return typeof(AssetState);
        }
    }
}