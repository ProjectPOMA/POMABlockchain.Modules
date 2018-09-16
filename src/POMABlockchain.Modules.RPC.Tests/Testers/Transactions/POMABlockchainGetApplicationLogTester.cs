using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Transactions;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Transactions
{
    public class POMABlockchainGetApplicationLogTester : RpcRequestTester<ApplicationLog>
    {
        [Fact]
        public async void ShouldReturnApplicationLog()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<ApplicationLog> ExecuteAsync(IClient client)
        {
            var applicationLog = new POMABlockchainGetApplicationLog(client);
            return await applicationLog.SendRequestAsync(Settings.GetContractTransaction());
        }

        public override Type GetRequestType()
        {
            return typeof(ApplicationLog);
        }
    }
}
