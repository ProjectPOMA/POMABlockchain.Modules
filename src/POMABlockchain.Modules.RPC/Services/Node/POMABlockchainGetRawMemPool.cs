using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Infrastructure;

namespace POMABlockchain.Modules.RPC.Services.Node
{
    /// <Summary>
    ///     getrawmempool   
    ///     Obtains the list of unconfirmed transactions in memory.
    /// 
    ///     Parameters
    ///     none
    /// 
    ///     Returns
    ///     Unconfirmed transactions received by nodes, i.e. transactions with zero confirmations.
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getrawmempool","params":[],"id":1}'
    /// 
    ///     Result
    ///     {
    ///    "jsonrpc": "2.0",
    ///    "id": 1,
    ///    "result": [
    ///      "B4534f6d4c17cda008a76a1968b7fa6256cd90ca448739eae8e828698ccc44e7"
    ///    ]
    /// }
    /// </Summary>
    public class POMABlockchainGetRawMemPool : GenericRpcRequestResponseHandlerNoParam<string[]>
    {
        public POMABlockchainGetRawMemPool(IClient client) : base(client, ApiMethods.getrawmempool.ToString())
        {
        }
    }
}