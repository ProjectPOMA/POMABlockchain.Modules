using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Account;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Account
{
    public class POMABlockchainDumpPrivateKeyTester : RpcRequestTester<string>
    {
        [Fact]
        public async void ShouldReturnPrivateKey()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<string> ExecuteAsync(IClient client)
        {
            var dumpPrivateKey = new POMABlockchainDumpPrivateKey(client);
            return await dumpPrivateKey.SendRequestAsync(Settings.GetDefaultAccount());
        }

        public override Type GetRequestType()
        {
            return typeof(string);
        }
    }
}
