using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Transactions;

namespace POMABlockchain.Modules.RPC.Services
{
	public class POMABlockchainApiTransactionService : RpcClientWrapper
	{
		public POMABlockchainApiTransactionService(IClient client) : base(client)
		{
			GetApplicationLog = new POMABlockchainGetApplicationLog(client);
			GetRawTransaction = new POMABlockchainGetRawTransaction(client);
			GetRawTransactionSerialized = new POMABlockchainGetRawTransactionSerialized(client);
			SendRawTransaction = new POMABlockchainSendRawTransaction(client);
			GetTransactionOutput = new POMABlockchainGetTransactionOutput(client);
			SendToAddress = new POMABlockchainSendToAddress(client);
			SendMany = new POMABlockchainSendMany(client);
			SendFrom = new POMABlockchainSendFrom(client);
		}

		public POMABlockchainGetApplicationLog GetApplicationLog { get; private set; }
		public POMABlockchainGetRawTransaction GetRawTransaction { get; private set; }
		public POMABlockchainGetRawTransactionSerialized GetRawTransactionSerialized { get; private set; }
		public POMABlockchainSendRawTransaction SendRawTransaction { get; private set; }
		public POMABlockchainGetTransactionOutput GetTransactionOutput { get; private set; }
		public POMABlockchainSendToAddress SendToAddress { get; private set; }
		public POMABlockchainSendMany SendMany { get; private set; }
		public POMABlockchainSendFrom SendFrom { get; private set; }
	}
}
