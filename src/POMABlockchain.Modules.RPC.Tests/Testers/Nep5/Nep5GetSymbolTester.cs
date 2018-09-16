using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Nep5;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Nep5
{
    public class Nep5GetSymbolTester : RpcRequestTester<Invoke>
    {
        [Fact]
        public async void ShouldReturnSymbol()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result.Stack[0].Value);
        }

        public override async Task<Invoke> ExecuteAsync(IClient client)
        {
            var symbol = new TokenSymbol(client);
            return await symbol.SendRequestAsync(Settings.GetNep5TokenHash());
        }

        public override Type GetRequestType()
        {
            return typeof(Invoke);
        }
    }
}
