using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Block;

namespace POMABlockchain.Modules.RPC.Services
{
    public class POMABlockchainApiBlockService : RpcClientWrapper
    {
        public POMABlockchainGetBlockHash GetBlockHash { get; private set; }
        public POMABlockchainGetBestBlockHash GetBestBlockHash { get; private set; }
        public POMABlockchainGetBlock GetBlock { get; private set; }
        public POMABlockchainGetBlockCount GetBlockCount { get; private set; }
        public POMABlockchainGetBlockSerialized GetBlockSerialized { get; private set; }
        public POMABlockchainGetBlockSysFee GetBlockSysFee { get; private set; }

        public POMABlockchainApiBlockService(IClient client) : base(client)
        {           
            GetBestBlockHash = new POMABlockchainGetBestBlockHash(client);
            GetBlock = new POMABlockchainGetBlock(client);
            GetBlockSerialized = new POMABlockchainGetBlockSerialized(client);
            GetBlockCount = new POMABlockchainGetBlockCount(client);
            GetBlockHash = new POMABlockchainGetBlockHash(client);
            GetBlockSysFee = new POMABlockchainGetBlockSysFee(client);
        }
    }
}
