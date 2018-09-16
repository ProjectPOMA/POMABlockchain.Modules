using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Account;

namespace POMABlockchain.Modules.RPC.Services
{
    public class POMABlockchainApiAccountService : RpcClientWrapper
    {
        public POMABlockchainApiAccountService(IClient client) : base(client)
        {
            GetAccountState = new POMABlockchainGetAccountState(client);
            ValidateAddress = new POMABlockchainValidateAddress(client);
            GetNewAddress = new POMABlockchainGetNewAddress(client);
            GetBalance = new POMABlockchainGetBalance(client);
            ListAddresses = new POMABlockchainListAddresses(client);
            DumpPrivateKey = new POMABlockchainDumpPrivateKey(client);
        }
       
        public POMABlockchainGetAccountState GetAccountState { get; private set; }
        public POMABlockchainValidateAddress ValidateAddress { get; private set; }
        public POMABlockchainGetNewAddress GetNewAddress { get; private set; }
        public POMABlockchainGetBalance GetBalance { get; private set; }
        public POMABlockchainListAddresses ListAddresses { get; private set; }
        public POMABlockchainDumpPrivateKey DumpPrivateKey { get; private set; }
    }
}
