using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Infrastructure;
using POMABlockchain.Modules.RPC.Services.Contract;
using POMABlockchain.Modules.RPC.Services.Transactions;

namespace POMABlockchain.Modules.RPC.TransactionManagers
{
    public abstract class TransactionManagerBase : ITransactionManager
    {
        public IClient Client { get; set; }
        public IAccount Account { get; set; }

        public abstract Task<string> SignTransactionAsync(byte[] transactionData);

        public abstract Task<Transaction> WaitForTxConfirmation(string tx);

        public virtual async Task<double> EstimateGasAsync(string serializedScriptHash)
        {
            if (Client == null) throw new NullReferenceException("Client not configured");
            if (serializedScriptHash == null) throw new ArgumentNullException(nameof(serializedScriptHash));
            var POMABlockchainEstimateGas = new POMABlockchainInvokeScript(Client);
            var invokeResult = await POMABlockchainEstimateGas.SendRequestAsync(serializedScriptHash);
            return double.Parse(invokeResult.GasConsumed, CultureInfo.InvariantCulture);
        }

        public virtual async Task<double> EstimateGasAsync(string scriptHash, string operation,
            List<InvokeParameter> parameterList)
        {
            if (Client == null) throw new NullReferenceException("Client not configured");
            if (scriptHash == null) throw new ArgumentNullException(nameof(scriptHash));
            var POMABlockchainEstimateGas = new POMABlockchainInvokeFunction(Client);
            var invokeResult = await POMABlockchainEstimateGas.SendRequestAsync(scriptHash, operation, parameterList);
            return double.Parse(invokeResult.GasConsumed, CultureInfo.InvariantCulture);
        } 

        public async Task<bool> SendTransactionAsync(string signedTx)
        {
            if (Client == null) throw new NullReferenceException("Client not configured");
            if (signedTx == null) throw new ArgumentNullException(nameof(signedTx));
            var POMABlockchainSendRawTransaction = new POMABlockchainSendRawTransaction(Client);
            return await POMABlockchainSendRawTransaction.SendRequestAsync(signedTx);
        }

        public async Task<Transaction> GetTransaction(string tx)
        {
            if (Client == null) throw new NullReferenceException("Client not configured");
            if (string.IsNullOrEmpty(tx)) throw new ArgumentNullException(nameof(tx));
            var POMABlockchainGetRawTransaction = new POMABlockchainGetRawTransaction(Client);
            return await POMABlockchainGetRawTransaction.SendRequestAsync(tx);
        }
    }
}