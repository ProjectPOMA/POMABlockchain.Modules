using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Asset;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Asset
{
    public class POMABlockchainGetAssetStateTester : RpcRequestTester<AssetState>
    {
        [Fact]
        public async void ShouldReturnAssetState()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<AssetState> ExecuteAsync(IClient client)
        {
            var assetState = new POMABlockchainGetAssetState(client);
            return await assetState.SendRequestAsync(Settings.GetGoverningAssetHash());
        }

        public override Type GetRequestType()
        {
            return typeof(AssetState);
        }
    }
}
