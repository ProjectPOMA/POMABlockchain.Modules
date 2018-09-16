using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Node;

namespace POMABlockchain.Modules.RPC.Services
{
    public class POMABlockchainApiNodeService : RpcClientWrapper
    {
        public POMABlockchainApiNodeService(IClient client) : base(client)
        {
            GetConnectionCount = new POMABlockchainGetConnectionCount(client);
            GetRawMemPool = new POMABlockchainGetRawMemPool(client);
            GetValidators = new POMABlockchainGetValidators(client);
            GetVersion = new POMABlockchainGetVersion(client);
        }

        public POMABlockchainGetConnectionCount GetConnectionCount { get; private set; }
        public POMABlockchainGetRawMemPool GetRawMemPool { get; private set; }
        public POMABlockchainGetVersion GetVersion { get; private set; }
        public POMABlockchainGetValidators GetValidators { get; private set; }
    }
}
