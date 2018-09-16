using POMABlockchain.Modules.RPC.TransactionManagers;

namespace POMABlockchain.Modules.RPC.Infrastructure
{
    public interface IAccount
    {
        string Address { get; }
        ITransactionManager TransactionManager { get; }
        byte[] PrivateKey { get; }
    }
}