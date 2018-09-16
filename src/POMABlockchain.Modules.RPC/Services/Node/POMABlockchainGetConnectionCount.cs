using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Infrastructure;

namespace POMABlockchain.Modules.RPC.Services.Node
{
    /// <Summary>
    ///     getconnectioncount   
    ///     Gets the current number of connections for the node.
    /// 
    ///     Parameters
    ///     none
    /// 
    ///     Returns
    ///     Number of connection on the node
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getconnectioncount","params":[],"id":1}'
    /// 
    ///     Result
    ///     {
    ///     "jsonrpc": "2.0",
    ///     "id": 1,
    ///     "result": 10
    /// }
    /// </Summary>
    public class POMABlockchainGetConnectionCount : GenericRpcRequestResponseHandlerNoParam<int>
    {
        public POMABlockchainGetConnectionCount(IClient client) : base(client, ApiMethods.getconnectioncount.ToString())
        {
        }
    }
}