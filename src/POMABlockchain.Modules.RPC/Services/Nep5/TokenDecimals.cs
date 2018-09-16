using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;

namespace POMABlockchain.Modules.RPC.Services.Nep5
{
    public class TokenDecimals : RpcRequestResponseHandler<DTOs.Invoke>
    {
        public TokenDecimals(IClient client) : base(client, ApiMethods.invokefunction.ToString())
        {
        }

        public Task<DTOs.Invoke> SendRequestAsync(string tokenScriptHash, object id = null)
        {
            return base.SendRequestAsync(id, tokenScriptHash, Nep5Methods.decimals.ToString());
        }

        public RpcRequest BuildRequest(string tokenScriptHash, object id = null)
        {
            return base.BuildRequest(id, tokenScriptHash, Nep5Methods.decimals.ToString());
        }
    }
}
