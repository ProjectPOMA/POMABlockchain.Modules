using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Block;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Block
{
    public class POMABlockchainGetBlockHashTester : RpcRequestTester<string>
    {
        [Fact]
        public async Task ShouldReturnBlockHash()
        {
            var result = await ExecuteAsync();
            Assert.True(!string.IsNullOrEmpty(result));
            Assert.StartsWith("0x", result);
        }

        public override async Task<string> ExecuteAsync(IClient client)
        {
            var blockHash = new POMABlockchainGetBlockHash(client);
            return await blockHash.SendRequestAsync(10000);
        }

        public override Type GetRequestType()
        {
            return typeof(string);
        }
    }
}
