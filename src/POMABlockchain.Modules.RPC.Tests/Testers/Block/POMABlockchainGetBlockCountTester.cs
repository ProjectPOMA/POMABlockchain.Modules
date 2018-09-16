using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Block;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Block
{
    public class POMABlockchainGetBlockCountTester : RpcRequestTester<int>
    {
        [Fact]
        public async Task ShouldReturnBlockCount()
        {
            var result = await ExecuteAsync();
            Assert.True(result > 0);
        }

        public override async Task<int> ExecuteAsync(IClient client)
        {
            var blockCount = new POMABlockchainGetBlockCount(client);
            return await blockCount.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(int);
        }
    }
}
