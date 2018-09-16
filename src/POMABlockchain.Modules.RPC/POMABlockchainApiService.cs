using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services;

namespace POMABlockchain.Modules.RPC
{
    public class POMABlockchainApiService : RpcClientWrapper
    {
        public POMABlockchainApiService(IClient client) : base(client)
        {
            Client = client;

            Accounts = new POMABlockchainApiAccountService(client);
            Assets = new POMABlockchainApiAssetService(client);
            Blocks = new POMABlockchainApiBlockService(client);
            Contracts = new POMABlockchainApiContractService(client);
            TokenStandard = new POMABlockchainNep5Service(client);
            Nodes = new POMABlockchainApiNodeService(client);
            Transactions = new POMABlockchainApiTransactionService(client);
        }

        public POMABlockchainApiAccountService Accounts { get; }
        public POMABlockchainApiAssetService Assets { get; }
        public POMABlockchainApiBlockService Blocks { get; }
        public POMABlockchainApiContractService Contracts { get; }
        public POMABlockchainNep5Service TokenStandard { get; }
        public POMABlockchainApiNodeService Nodes { get; }
        public POMABlockchainApiTransactionService Transactions { get; }
    }
}