using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Block;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Block
{
    public class POMABlockchainGetBestBlockHashTester : RpcRequestTester<string>
    {
        [Fact]
        public async void ShouldReturnBestBlockHash()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
            Assert.StartsWith("0x",result);
        }

        public override async Task<string> ExecuteAsync(IClient client)
        {
            var bestBlockHash = new POMABlockchainGetBestBlockHash(client);
            return await bestBlockHash.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(string);
        }
    }
}
