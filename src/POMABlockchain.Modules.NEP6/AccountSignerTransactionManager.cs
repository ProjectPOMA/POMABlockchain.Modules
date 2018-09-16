﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POMABlockchain.Modules.Core;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.KeyPairs;
using POMABlockchain.Modules.NEP6.Helpers;
using POMABlockchain.Modules.NEP6.Interfaces;
using POMABlockchain.Modules.NEP6.Transactions;
using POMABlockchain.Modules.Rest.Interfaces;
using POMABlockchain.Modules.Rest.Services;
using POMABlockchain.Modules.RPC;
using POMABlockchain.Modules.RPC.DTOs;
using POMABlockchain.Modules.RPC.Infrastructure;
using POMABlockchain.Modules.RPC.TransactionManagers;
using Org.BouncyCastle.Security;
using Helper = POMABlockchain.Modules.KeyPairs.Helper;

namespace POMABlockchain.Modules.NEP6
{
    public class AccountSignerTransactionManager : TransactionManagerBase, IRandomNumberGenerator
    {
        private static readonly SecureRandom Random = new SecureRandom();
        private readonly KeyPair _accountKey;
        private readonly IPOMABlockchainscanService _restService;

        public AccountSignerTransactionManager(IClient rpcClient, IPOMABlockchainscanService restService, IAccount account)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
            Client = rpcClient;
            _restService = restService;
            if (account.PrivateKey != null)
                _accountKey = new KeyPair(account.PrivateKey); //if account is watch only, it does not have private key
        }

        public byte[] GenerateNonce(int size)
        {
            var bytes = new byte[size];
            Random.NextBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// Signs a SignedTransaction object and sends a 'sendrawtransaction' RPC call to the connected node.
        /// </summary>
        /// <param name="txInput"></param>
        /// <returns></returns>
        private async Task<bool> SignAndSendTransaction(SignedTransaction txInput)
        {
            if (txInput == null) return false;
            SignTransaction(txInput);
            var serializedTransaction = txInput.Serialize();
            return await SendTransactionAsync(serializedTransaction.ToHexString());
        }

        /// <summary>
        /// Signs a SignedTransaction object
        /// </summary>
        /// <param name="txInput"></param>
        public void SignTransaction(SignedTransaction txInput)
        {
            txInput.Sign(_accountKey);
        }

        /// <summary>
        /// (Alternative) Signs a byte array that has the transaction data.
        /// </summary>
        /// <param name="transactionData"></param>
        /// <returns></returns>
        public override Task<string> SignTransactionAsync(byte[] transactionData) //todo refractor this to have more use
        {
            var signerTransaction = Utils.Sign(transactionData, _accountKey.PrivateKey);
            return Task.FromResult(signerTransaction.ToHexString());
        }


        /// <summary>
        /// Makes a 'getrawtransaction ' RPC call to the connected node.
        /// Only returns if the Transaction already has a block hash (indicates that is part of a block, therefore is confirmed)
        /// </summary>
        /// <param name="tx"></param>
        /// <returns></returns>
        public override async Task<Transaction> WaitForTxConfirmation(string tx)
        {
            while (true)
            {
                var confirmedTx = await GetTransaction(tx);
                if (confirmedTx != null && !string.IsNullOrEmpty(confirmedTx.Blockhash)) return confirmedTx;
                await Task.Delay(10000);
            }
        }

        #region Contracts

        /// <summary>
        /// Makes a 'invokescript' RPC call to the connected node.
        /// Return the gas cost if the contract tx is "simulated" correctly
        /// </summary>
        /// <param name="scriptHash"></param>
        /// <param name="operation"></param>
        /// <param name="args"></param>
        /// <param name="attachSymbol"></param>
        /// <param name="attachTargets"></param>
        /// <returns></returns>
        public async Task<double> EstimateGasContractCall(byte[] scriptHash, string operation,
            object[] args, string attachSymbol = null, IEnumerable<TransferOutput> attachTargets = null)
        {
            var bytes = Utils.GenerateScript(scriptHash, operation, args);

            if (string.IsNullOrEmpty(attachSymbol)) attachSymbol = "GAS";

            if (attachTargets == null) attachTargets = new List<TransferOutput>();

            var (inputs, outputs) =
                await TransactionBuilderHelper.GenerateInputsOutputs(_accountKey, attachSymbol, attachTargets, 0, _restService);

            if (inputs.Count == 0) throw new WalletException($"Not enough inputs for transaction");

            var tx = new SignedTransaction
            {
                Type = TransactionType.InvocationTransaction,
                Version = 0,
                Script = bytes,
                Gas = 0,
                Inputs = inputs.ToArray(),
                Outputs = outputs.ToArray()
            };

            SignTransaction(tx);
            var serializedScriptHash = tx.Serialize();
            return await EstimateGasAsync(serializedScriptHash.ToHexString());
        }

        /// <summary>
        /// Creates a 'InvocationTransaction' with the parameters passed, signs it and send a 'sendrawtransaction' RPC call to the connected node.
        /// </summary>
        /// <param name="contractScriptHash"></param>
        /// <param name="operation"></param>
        /// <param name="args"></param>
        /// <param name="attachSymbol"></param>
        /// <param name="attachTargets"></param>
        /// <returns></returns>
        public async Task<SignedTransaction> CallContract(byte[] contractScriptHash, string operation,
            object[] args, string attachSymbol = null, IEnumerable<TransferOutput> attachTargets = null)
        {
            var bytes = Utils.GenerateScript(contractScriptHash, operation, args);
            return await CallContract(_accountKey, contractScriptHash, bytes, attachSymbol, attachTargets);
        }

        /// <summary>
        /// (Alternative)
        /// Creates a 'InvocationTransaction' with the parameters passed, signs it and send a 'sendrawtransaction' RPC call to the connected node.
        /// But because there are no fees currently, you can execute contracts without assets, if there is no need for input.
        /// </summary>
        /// <param name="scriptHash"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        public async Task<SignedTransaction> AssetlessContractCall(byte[] scriptHash, byte[] script)
        {
            var signatureScript = Helper.CreateSignatureRedeemScript(_accountKey.PublicKey).ToScriptHash();
            var tx = new SignedTransaction
            {
                Type = TransactionType.InvocationTransaction,
                Version = 0,
                Script = script,
                Gas = 0,
                Inputs = new SignedTransaction.Input[0],
                Outputs = new SignedTransaction.Output[0],
                Attributes = new[]
                {
                    new TransactionAttribute
                    {
                        Data = signatureScript.ToArray(),
                        Usage = TransactionAttributeUsage.Script
                    },
                    new TransactionAttribute //TODO: change this to use the same nonce in same tx to avoid duplicated tx
                    {
                        Data = GenerateNonce(4),
                        Usage = TransactionAttributeUsage.Remark
                    }
                }
            };
            var result = await SignAndSendTransaction(tx);
            return result ? tx : null;
        }

        public async Task<SignedTransaction> CallContract(KeyPair key, byte[] scriptHash, byte[] script,
            string attachSymbol = null, IEnumerable<TransferOutput> attachTargets = null, decimal fee = 0)
        {
            if (string.IsNullOrEmpty(attachSymbol)) attachSymbol = "GAS";

            if (attachTargets == null) attachTargets = new List<TransferOutput>();

            var (inputs, outputs) =
                await TransactionBuilderHelper.GenerateInputsOutputs(key, attachSymbol, attachTargets, fee, _restService);

            //Assetless contract call
            if (inputs.Count == 0 && outputs.Count == 0)
            {
                return await AssetlessContractCall(scriptHash, script);
            }

            var tx = new SignedTransaction
            {
                Type = TransactionType.InvocationTransaction,
                Version = 0,
                Script = script,
                Gas = 0,
                Inputs = inputs.ToArray(),
                Outputs = outputs.ToArray()
            };

            var result = await SignAndSendTransaction(tx);
            return result ? tx : null;
        }

        #endregion

        #region Assets

        /// <summary>
        /// Sends a 'native' asset (POMABlockchain or Gas) to another address using 'sendrawtransaction'.
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="symbol"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<SignedTransaction> SendAsset(string toAddress, Dictionary<string, decimal> symbolsAndAmount, decimal fee = 0)
        {
            var toScriptHash = toAddress.ToScriptHash().ToArray();
            var targets = TransactionBuilderHelper.BuildTransferOutputs(toAddress, symbolsAndAmount);
            //var fixedFee = fee == 0 ? Fixed8.Zero : Fixed8.FromDecimal(fee);
            return await SendAsset(_accountKey, targets, fee);
        }

        /// <summary>
        /// Sends a 'native' asset (POMABlockchain or Gas) to another address using 'sendrawtransaction'.
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="symbol"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<SignedTransaction> SendAsset(byte[] toAddress, List<string> symbols, decimal amount, decimal fee = 0)
        {
            var target = new TransferOutput { AddressHash = toAddress, Amount = amount };
            var targets = new List<TransferOutput> { target };
            //var fixedFee = fee == 0 ? Fixed8.Zero : Fixed8.FromDecimal(fee);
            return await SendAsset(_accountKey, targets, fee);
        }

        public async Task<SignedTransaction> SendAsset(KeyPair fromKey, IEnumerable<TransferOutput> targets, decimal fee)
        {
            List<SignedTransaction.Input> finalInputs = new List<SignedTransaction.Input>();
            List<SignedTransaction.Output> finalOutputs = new List<SignedTransaction.Output>();

            foreach (var transferOutput in targets)
            {
                List<SignedTransaction.Input> inputs;
                List<SignedTransaction.Output> outputs;

                (inputs, outputs) =
                    await TransactionBuilderHelper.GenerateInputsOutputs(fromKey,
                        transferOutput.Symbol,
                        new List<TransferOutput> { transferOutput },
                        fee,
                        _restService);
                finalInputs.AddRange(inputs);
                finalOutputs.AddRange(outputs);
            }

            var tx = new SignedTransaction
            {
                Type = TransactionType.ContractTransaction,
                Version = 0,
                Script = null,
                Gas = -1,
                Inputs = finalInputs.ToArray(),
                Outputs = finalOutputs.ToArray()
            };

            var result = await SignAndSendTransaction(tx);
            return result ? tx : null;
        }
        /// <summary>
        /// Creates a 'ClaimTransaction', signs it and send a 'sendrawtransaction' RPC call to the connected node.
        /// </summary>
        /// <returns></returns>
        public async Task<SignedTransaction> ClaimGas()
        {
            var address = Helper.CreateSignatureRedeemScript(_accountKey.PublicKey);
            var targetScriptHash = address.ToScriptHash();
            var (claimable, amount) =
                await TransactionBuilderHelper.GetClaimable(address.ToScriptHash().ToAddress(), _restService);

            var references = new List<SignedTransaction.Input>();
            foreach (var entry in claimable)
                references.Add(new SignedTransaction.Input
                {
                    PrevHash = entry.Txid.HexToBytes().Reverse().ToArray(),
                    PrevIndex = entry.N
                });

            if (amount <= 0) throw new WalletException("No GAS available to claim at this address");

            var outputs = new List<SignedTransaction.Output>
            {
                new SignedTransaction.Output
                {
                    ScriptHash = targetScriptHash.ToArray(),
                    AssetId = Utils.GasToken.HexToBytes().Reverse().ToArray(),
                    Value = amount
                }
            };

            var tx = new SignedTransaction
            {
                Type = TransactionType.ClaimTransaction,
                Version = 0,
                Script = null,
                Gas = -1,
                References = references.ToArray(),
                Inputs = new SignedTransaction.Input[0],
                Outputs = outputs.ToArray()
            };

            tx.Sign(_accountKey);

            var result = await SignAndSendTransaction(tx);
            return result ? tx : null;
        }

        #endregion

        #region NEP5 Transfer

        /// <summary>
        /// Creates a 'InvocationTransaction' with the parameters passed, signs it and send a 'sendrawtransaction' RPC call to the connected node.
        /// Used the NEP5 standard 'tranfer' method.
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="amount"></param>
        /// <param name="tokenScriptHash"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public async Task<SignedTransaction> TransferNep5(string toAddress, decimal amount, byte[] tokenScriptHash,
            int decimals = 8)
        {
            var toAddressScriptHash = toAddress.ToScriptHash().ToArray();
            return await TransferNep5(toAddressScriptHash, amount, tokenScriptHash, decimals);
        }

        public async Task<SignedTransaction> TransferNep5(byte[] toAddress, decimal amount, byte[] tokenScriptHash,
            int decimals = 8)
        {
            if (toAddress.Length != 20) throw new ArgumentException(nameof(toAddress));
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));

            var keyAddress = Helper.CreateSignatureRedeemScript(_accountKey.PublicKey);
            var fromAddress = keyAddress.ToScriptHash().ToArray();
            var amountBigInteger = Utils.ConvertToBigInt(amount, decimals);

            var result = await CallContract(tokenScriptHash,
                Nep5Methods.transfer.ToString(),
                new object[] { fromAddress, toAddress, amountBigInteger });

            return result;
        }
        #endregion
    }
}