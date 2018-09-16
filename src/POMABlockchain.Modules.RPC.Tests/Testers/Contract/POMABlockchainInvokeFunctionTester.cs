using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Contract;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Contract
{
    public class POMABlockchainInvokeFunctionTester : RpcRequestTester<Invoke>
    {
        [Fact]
        public async void ShouldReturnInvokeFunctionResult()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<Invoke> ExecuteAsync(IClient client)
        {
            var invokeFunction = new POMABlockchainInvokeFunction(client);
            var parametersList = new List<InvokeParameter>
            {
                new InvokeParameter
                {
                    Type = "Hash160",
                    Value = "bfc469dd56932409677278f6b7422f3e1f34481d"
                }
            };
            return await invokeFunction.SendRequestAsync("ecc6b20d3ccac1ee9ef109af5a7cdb85706b1df9", "balanceOf", parametersList);
        }

        public override Type GetRequestType()
        {
            return typeof(Invoke);
        }
    }
}
