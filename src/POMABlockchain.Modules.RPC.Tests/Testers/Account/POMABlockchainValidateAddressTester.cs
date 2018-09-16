using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Services.Account;
using Xunit;

namespace POMABlockchain.Modules.RPC.Tests.Testers.Account
{
    public class POMABlockchainValidateAddressTester : RpcRequestTester<ValidateAddress>
    {
        private string InvalidAddress { get; } = "thisIsAnInvalidAddress";

        [Fact]
        public async void ShouldReturnValid()
        {
            var validAddress = await ExecuteAsync();
            Assert.True(validAddress != null && validAddress.IsValid);
        }

        [Fact]
        public async void ShouldReturnInvalid()
        {
            var validateAddress = new POMABlockchainValidateAddress(Client);
            var invalidAddress = await validateAddress.SendRequestAsync(InvalidAddress);
            Assert.False(invalidAddress != null && invalidAddress.IsValid);
        }

        public override async Task<ValidateAddress> ExecuteAsync(IClient client)
        {
            var validateAddress = new POMABlockchainValidateAddress(client);
            return await validateAddress.SendRequestAsync(Settings.GetDefaultAccount());
        }

        public override Type GetRequestType()
        {
            return typeof(ValidateAddress);
        }
    }
}
