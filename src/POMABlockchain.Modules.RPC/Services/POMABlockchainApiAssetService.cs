using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Asset;

namespace POMABlockchain.Modules.RPC.Services
{
    public class POMABlockchainApiAssetService : RpcClientWrapper
    {
        public POMABlockchainApiAssetService(IClient client) : base(client)
        {
            GetAssetState = new POMABlockchainGetAssetState(client);
        }

        public POMABlockchainGetAssetState GetAssetState { get; private set; }
    }
}
