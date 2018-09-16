using System;
    using System.Numerics;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Infrastructure;
using POMABlockchain.Modules.RPC.Services.Nep5;

namespace POMABlockchain.Modules.RPC.Services
{
    public class POMABlockchainNep5Service : RpcClientWrapper
    {
        public POMABlockchainNep5Service(IClient client) : base(client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            GetTokenBalance = new TokenBalanceOf(client);
            GetTokenDecimals = new TokenDecimals(client);
            GetTokenName = new TokenName(client);
            GetTokenTotalSupply = new TokenTotalSupply(client);
            GetTokenSymbol = new TokenSymbol(client);
        }

        private TokenBalanceOf GetTokenBalance { get; }
        private TokenDecimals GetTokenDecimals { get; }
        private TokenName GetTokenName { get; }
        private TokenTotalSupply GetTokenTotalSupply { get; }
        private TokenSymbol GetTokenSymbol { get; }

        public async Task<string> GetName(string tokenScriptHash, bool humanReadable = false)
        {
            var result = await GetTokenName.SendRequestAsync(tokenScriptHash);

            if (result.State == "FAULT, BREAK")
                throw new RpcResponseException(new RpcError(0, "RPC state response: FAULT, BREAK "));

            var resultString = result.Stack[0].Value.ToString();
            if (!humanReadable) return resultString;
            return resultString.HexToString();
        }

        public async Task<string> GetSymbol(string tokenScriptHash, bool humanReadable = false)
        {
            var result = await GetTokenSymbol.SendRequestAsync(tokenScriptHash);

            if (result.State == "FAULT, BREAK")
                throw new RpcResponseException(new RpcError(0, "RPC state response: FAULT, BREAK "));

            var resultString = result.Stack[0].Value.ToString();
            if (!humanReadable) return resultString;
            return resultString.HexToString();
        }

        public async Task<decimal> GetTotalSupply(string tokenScriptHash, int decimals = 8)
        {
            var result = await GetTokenTotalSupply.SendRequestAsync(tokenScriptHash);

            if (result.State == "FAULT, BREAK")
                throw new RpcResponseException(new RpcError(0, "RPC state response: FAULT, BREAK "));

            var resultString = result.Stack[0].Value.ToString();
            if (decimals.Equals(0)) return Convert.ToInt64(resultString, 16);

            if (decimals.Equals(0)) return Convert.ToInt64(resultString, 16);

            var totalSupplyBigInteger = new BigInteger(resultString.HexStringToBytes());
            var totalSupply = Nep5Helper.CalculateDecimalFromBigInteger(totalSupplyBigInteger, decimals);

            return totalSupply;
        }

        public async Task<int> GetDecimals(string tokenScriptHash)
        {
            var result = await GetTokenDecimals.SendRequestAsync(tokenScriptHash);

            if (result.State == "FAULT, BREAK")
                throw new RpcResponseException(new RpcError(0, "RPC state response: FAULT, BREAK "));

            return int.Parse(result.Stack[0].Value.ToString());
        }

        public async Task<decimal> GetBalance(string tokenScriptHash, string addressScriptHash, int decimals = 0)
        {
            var result = await GetTokenBalance.SendRequestAsync(addressScriptHash, tokenScriptHash);

            if (result.State == "FAULT, BREAK")
                throw new RpcResponseException(new RpcError(0, "RPC state response: FAULT, BREAK "));

            var resultString = result.Stack[0].Value.ToString();
            if (decimals.Equals(0)) return Convert.ToInt64(resultString, 16);

            var balanceBigInteger = new BigInteger(resultString.HexStringToBytes());
            var balance =  Nep5Helper.CalculateDecimalFromBigInteger(balanceBigInteger, decimals);

            return balance;
        }
    }
}