using System;
using POMABlockchain.Modules.JsonRpc.Client;

namespace POMABlockchain.Modules.RPC.Tests
{
    public class ClientFactory
    {
        public static IClient GetClient(TestSettings settings)
        {
            var url = settings.GetRpcUrl();
            return new RpcClient(new Uri(url));
        }
    }
}
