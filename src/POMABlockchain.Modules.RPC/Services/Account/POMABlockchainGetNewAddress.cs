using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Infrastructure;

namespace POMABlockchain.Modules.RPC.Services.Account
{
    /// <summary>
    ///     getnewaddress
    ///     create a new address
    /// 
    ///     Parameters
    ///     None
    /// 
    ///     Returns
    ///     newly generated address
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getnewaddress","params":[],"id":1}'
    /// 
    ///     Result
    ///     {
    ///     "jsonrpc": "2.0",
    ///     "id": 1,
    ///     "result": "AVHcdW3FGKbPWGHNhkPjgVgi4GGndiCxdo"
    /// }
    /// </summary>
    public class POMABlockchainGetNewAddress : GenericRpcRequestResponseHandlerNoParam<string>
	{
        public POMABlockchainGetNewAddress(IClient client) : base(client, ApiMethods.getnewaddress.ToString())
        {
        }
    }
}