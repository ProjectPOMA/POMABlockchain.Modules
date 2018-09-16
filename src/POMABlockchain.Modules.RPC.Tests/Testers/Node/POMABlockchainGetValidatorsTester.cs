using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Node;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Node
{
    public class POMABlockchainGetValidatorsTester : RpcRequestTester<List<Validator>>
    {
        public override async Task<List<Validator>> ExecuteAsync(IClient client)
        {
            var validators = new POMABlockchainGetValidators(client);
            return await validators.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(List<Validator>);
        }

        [Fact]
        public async void ShouldReturnValidators()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}