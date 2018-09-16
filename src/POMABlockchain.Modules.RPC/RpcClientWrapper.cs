using System;
using POMABlockchain.Modules.JsonRpc.Client;

namespace POMABlockchain.Modules.RPC
{
    public class RpcClientWrapper
    {
        public RpcClientWrapper(IClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        protected IClient Client { get; set; }

        public void SwitchClient(IClient client)
        {
            Client = client;
        }
    }
}
