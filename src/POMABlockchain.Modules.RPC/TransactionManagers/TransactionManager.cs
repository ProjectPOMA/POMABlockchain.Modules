﻿using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;

namespace POMABlockchain.Modules.RPC.TransactionManagers
{
    public class TransactionManager : TransactionManagerBase
    {
        public TransactionManager(IClient client)
        {
            Client = client;
        }

        public override Task<string> SignTransactionAsync(byte[] transactionData)
        {
            throw new System.NotImplementedException();
        }

        public override Task<Transaction> WaitForTxConfirmation(string tx)
        {
            throw new System.NotImplementedException();
        }
    }
}