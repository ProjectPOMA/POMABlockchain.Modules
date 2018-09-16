using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Infrastructure;

namespace POMABlockchain.Modules.RPC.Services.Block
{
    /// <Summary>
    ///     getbestblockhash 
    ///     Returns the hash of the tallest block in the main chain.
    /// 
    ///     Parameters
    ///     none
    /// 
    ///     Returns
    ///     The hash of the tallest block in the main chain.
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getbestblockhash","params":[],"id":1}'
    /// 
    ///     Result
    ///     {
    ///    "jsonrpc": "2.0",
    ///    "id": 1,
    ///    "result": "773dd2dae4a9c9275290f89b56e67d7363ea4826dfd4fc13cc01cf73a44b0d0e"
    ///     }
    /// </Summary>
    public class POMABlockchainGetBestBlockHash : GenericRpcRequestResponseHandlerNoParam<string>
    {
        public POMABlockchainGetBestBlockHash(IClient client) : base(client, ApiMethods.getbestblockhash.ToString())
        {
        }
    }
}