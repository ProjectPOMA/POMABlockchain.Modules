using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;

namespace POMABlockchain.Modules.RPC.Tests.Testers
{
    public abstract class RpcRequestTester<T> : IRpcRequestTester
    {
        public IClient Client { get; set; }
        public TestSettings Settings { get; set; }

        protected RpcRequestTester()
        {
            Settings = new TestSettings();
            Client = ClientFactory.GetClient(Settings);
        }

        public Task<T> ExecuteAsync()
        {
            return ExecuteAsync(Client);
        }

        public async Task<object> ExecuteTestAsync(IClient client)
        {
            return await ExecuteAsync(client);
        }

        public abstract Task<T> ExecuteAsync(IClient client);

        public abstract Type GetRequestType();
    }
}