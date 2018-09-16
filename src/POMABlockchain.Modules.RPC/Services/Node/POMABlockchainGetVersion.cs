using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Infrastructure;

namespace POMABlockchain.Modules.RPC.Services.Node
{
    /// <Summary>
    ///     getversion    
    ///     Returns the version information about the queried node.
    /// 
    ///     Parameters
    ///     none
    /// 
    ///     Returns
    ///     Information about node version
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getversion","params":[],"id":1}'
    /// 
    ///     Result
    ///     {
    ///         "jsonrpc": "2.0",
    ///         "id": 3,
    ///         "result": {
    ///             "port": 0,
    ///             "nonce": 156443862,
    ///             "useragent": "/POMABlockchain:2.3.5/"
    ///         }
    ///     }
    /// </Summary>
    public class POMABlockchainGetVersion : GenericRpcRequestResponseHandlerNoParam<DTOs.Version>
    {
        public POMABlockchainGetVersion(IClient client) : base(client, ApiMethods.getversion.ToString())
        {
        }
    }
}