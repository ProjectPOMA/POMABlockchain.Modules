using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Contract;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Contract
{
    public class POMABlockchainInvokeTester : RpcRequestTester<Invoke>
    {
        [Fact]
        public async void ShouldReturnInvokeResult()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<Invoke> ExecuteAsync(IClient client)
        {
            var invoke = new POMABlockchainInvoke(client);
            var parametersList = new List<InvokeParameter>
            {
                new InvokeParameter
                {
                    Type = "String",
                    Value = "name"
                },
                new InvokeParameter
                {
                    Type = "Boolean",
                    Value = "false"
                }
            };
            return await invoke.SendRequestAsync("dc675afc61a7c0f7b3d2682bf6e1d8ed865a0e5f", parametersList);
        }

        public override Type GetRequestType()
        {
            return typeof(Invoke);
        }
    }
}
