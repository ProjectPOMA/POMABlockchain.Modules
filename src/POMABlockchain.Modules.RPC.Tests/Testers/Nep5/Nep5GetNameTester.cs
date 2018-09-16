using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Nep5;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Nep5
{
    public class Nep5GetNameTester : RpcRequestTester<Invoke>
    {
        [Fact]
        public async void ShouldReturnName()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result.Stack[0].Value);
        }

        public override async Task<Invoke> ExecuteAsync(IClient client)
        {
            var name = new TokenName(client);
            return await name.SendRequestAsync(Settings.GetNep5TokenHash());
        }

        public override Type GetRequestType()
        {
            return typeof(Invoke);
        }
    }
}
