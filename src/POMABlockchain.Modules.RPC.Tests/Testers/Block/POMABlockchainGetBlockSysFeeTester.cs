using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Block;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Block
{
    public class POMABlockchainGetBlockSysFeeTester : RpcRequestTester<string>
    {
        [Fact]
        public async void ShouldReturnBlockSysFee()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<string> ExecuteAsync(IClient client)
        {
            var blockSysFee = new POMABlockchainGetBlockSysFee(client);
            return await blockSysFee.SendRequestAsync(1005434);
        }

        public override Type GetRequestType()
        {
            return typeof(string);
        }
    }
}
