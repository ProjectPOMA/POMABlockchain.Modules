using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;

namespace POMABlockchain.Modules.RPC.Tests.Testers
{
    public interface IRpcRequestTester
    {
        Task<object> ExecuteTestAsync(IClient client);
        Type GetRequestType();
    }
}