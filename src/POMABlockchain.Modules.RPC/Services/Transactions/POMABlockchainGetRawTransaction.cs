using System;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;

namespace POMABlockchain.Modules.RPC.Services.Transactions
{
	/// <Summary>
	///     getrawtransaction    
	///     Returns the corresponding transaction information, based on the specified hash value.
	/// 
	///     Parameters
	///     Txid: Transaction ID
	///     Verbose: Optional, the default value of verbose is 0. When verbose is 0, the serialized information of the block is returned, represented by a hexadecimal string. 
	///     If you need to get detailed information, you will need to use the SDK for deserialization. When verbose is 1, detailed information of the corresponding block in Json format string, is returned.
	/// 
	///     Returns
	///     Transaction object
	/// 
	///     Example
	/// 
	///     Request
	///     curl -X POST --data '{"jsonrpc":"2.0","method":"getblock","params":[f4250dab094c38d8265acc15c366dc508d2e14bf5699e12d9df26577ed74d657, 1],"id":1}'
	/// 
	///     Result
	///     {
	///     "jsonrpc": "2.0",
	///     "id": 1,
	///     "result":{
	///   "jsonrpc": "2.0",
	///   "id": 1,
	///   "result": {
	///     "Txid": "f4250dab094c38d8265acc15c366dc508d2e14bf5699e12d9df26577ed74d657",
	///     "Size": 262,
	///     "Type": "ContractTransaction",
	///     "Version": 0,
	///     "Attributes":[],
	///     "Vin": [
	///       {
	///         "Txid": "abe82713f756eaeebf6fa6440057fca7c36b6c157700738bc34d3634cb765819",
	///         "Vout": 0
	///       }
	///      ],
	///      "Vout": [
	///       {
	///         "N": 0,
	///         "Asset": "c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b",
	///         "Value": "2950",
	///         "Address": "AHCNSDkh2Xs66SzmyKGdoDKY752uyeXDrt"
	///       },
	///       {
	///         "N": 1,
	///         "Asset": "c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b",
	///         "Value": "4050",
	///         "Address": "ALDCagdWUVV4wYoEzCcJ4dtHqtWhsNEEaR"
	///        }
	///     ],
	///     "Sys_fee": "0",
	///     "Net_fee": "0",
	///     "Scripts": [
	///       {
	///         "Invocation": "40915467ecd359684b2dc358024ca750609591aa731a0b309c7fb3cab5cd0836ad3992aa0a24da431f43b68883ea5651d548feb6bd3c8e16376e6e426f91f84c58",
	///         "Verification": "2103322f35c7819267e721335948d385fae5be66e7ba8c748ac15467dcca0693692dac"
	///       }
	///     ],
	///     "Blockhash": "9c814276156d33f5dbd4e1bd4e279bb4da4ca73ea7b7f9f0833231854648a72c",
	///     "Confirmations": 144,
	///     "Blocktime": 1496719422
	///   }
	/// }
	/// </Summary>
	public class POMABlockchainGetRawTransaction : RpcRequestResponseHandler<DTOs.Transaction>
	{
		public POMABlockchainGetRawTransaction(IClient client) : base(client, ApiMethods.getrawtransaction.ToString())
		{
		}

		public Task<DTOs.Transaction> SendRequestAsync(string txId, object id = null)
		{
			if (txId == null) throw new ArgumentNullException(nameof(txId));
			return base.SendRequestAsync(id, txId, 1);
		}

		public RpcRequest BuildRequest(string txId, object id = null)
		{
			if (txId == null) throw new ArgumentNullException(nameof(txId));
			return base.BuildRequest(id, txId, 1);
		}
	}
}