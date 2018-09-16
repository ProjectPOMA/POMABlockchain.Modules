using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Contract;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Contract
{
    public class POMABlockchainInvokeScriptTester : RpcRequestTester<Invoke>
    {
        [Fact]
        public async void ShouldReturnInvokeScriptResult()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<Invoke> ExecuteAsync(IClient client)
        {
            var invokeScript = new POMABlockchainInvokeScript(client);
            return await invokeScript.SendRequestAsync(Settings.GetContractHash());
        }

        public override Type GetRequestType()
        {
            return typeof(Invoke);
        }
    }
}
