﻿using System.Collections.Generic;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Infrastructure;

namespace POMABlockchain.Modules.RPC.TransactionManagers
{
    public interface ITransactionManager
    {
        IClient Client { get; set; }
        IAccount Account { get; set; }
        Task<double> EstimateGasAsync(string serializedScriptHash);
        Task<double> EstimateGasAsync(string scriptHash, string operation, List<InvokeParameter> parameterList);
        Task<bool> SendTransactionAsync(string serializedAndSignedTx);
        Task<string> SignTransactionAsync(byte[] transactionData); //TODO change byte[] a specific object
        Task<Transaction> GetTransaction(string tx);
    }
}