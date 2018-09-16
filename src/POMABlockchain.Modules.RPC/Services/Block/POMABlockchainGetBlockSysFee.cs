using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;

namespace POMABlockchain.Modules.RPC.Services.Block
{
    /// <Summary>
    ///     getblocksysfee  
    ///     Returns the system fees of the block, based on the specified index.
    /// 
    ///     Parameters
    ///     Index：Block index
    /// 
    ///     Returns
    ///     The system fees of the block, in POMABlockchainGas units.
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getblocksysfee","params":[1005434],"id":1}'
    /// 
    ///     Result
    ///     {
    ///     "jsonrpc": "2.0",
    ///     "id": 1,
    ///     "result": "195500"
    ///     }
    /// </Summary>
    public class POMABlockchainGetBlockSysFee : RpcRequestResponseHandler<string>
    {
        public POMABlockchainGetBlockSysFee(IClient client) : base(client, ApiMethods.getblocksysfee.ToString())
        {
        }

        public Task<string> SendRequestAsync(int blockIndex, object id = null)
        {
            if (blockIndex < 0) throw new ArgumentOutOfRangeException(nameof(blockIndex));
            return base.SendRequestAsync(id, blockIndex);
        }

        public RpcRequest BuildRequest(int blockIndex, object id = null)
        {
            if (blockIndex < 0) throw new ArgumentOutOfRangeException(nameof(blockIndex));
            return base.BuildRequest(id, blockIndex);
        }
    }
}