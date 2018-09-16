using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Block;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Block
{
    public class POMABlockchainGetBlockTester : RpcRequestTester<DTOs.Block>
    {
        [Fact]
        public async void ShouldReturnBlockWithHash()
        {
            var getBlock = new POMABlockchainGetBlock(Client);
            var blockByIndex = await getBlock.SendRequestAsync(Settings.GetBlockHash());
            Assert.NotNull(blockByIndex);
        }

        [Fact]
        public async Task ShouldReturnBlockWithIndex()
        {
            var blockByIndex = await ExecuteAsync();
            Assert.NotNull(blockByIndex);
        }

        public override async Task<DTOs.Block> ExecuteAsync(IClient client)
        {
            var block = new POMABlockchainGetBlock(client);
            return await block.SendRequestAsync(1);
        }

        public override Type GetRequestType()
        {
            return typeof(DTOs.Block);
        }
    }
}
