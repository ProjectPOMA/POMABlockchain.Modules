using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.Services.Contract;

namespace POMABlockchain.Modules.RPC.Services
{
	public class POMABlockchainApiContractService : RpcClientWrapper
	{
		public POMABlockchainApiContractService(IClient client) : base(client)
		{
			GetContractState = new POMABlockchainGetContractState(client);
			GetStorage = new POMABlockchainGetStorage(client);
			Invoke = new POMABlockchainInvoke(client);
			InvokeFunction = new POMABlockchainInvokeFunction(client);
			InvokeScript = new POMABlockchainInvokeScript(client);
		}

		public POMABlockchainGetContractState GetContractState { get; set; }

		public POMABlockchainGetStorage GetStorage { get; set; }

		public POMABlockchainInvoke Invoke { get; set; }

		public POMABlockchainInvokeFunction InvokeFunction { get; set; }

		public POMABlockchainInvokeScript InvokeScript { get; set; }
	}
}
