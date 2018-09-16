using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Block;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Block
{
    public class POMABlockchainGetBlockSerializedTester : RpcRequestTester<string>
    {
        [Fact]
        public async void ShouldReturnBlockSerialized()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<string> ExecuteAsync(IClient client)
        {
            var blockSerialized = new POMABlockchainGetBlockSerialized(client);
            return await blockSerialized.SendRequestAsync(1005434);
        }

        public override Type GetRequestType()
        {
            return typeof(string);
        }
    }
}
