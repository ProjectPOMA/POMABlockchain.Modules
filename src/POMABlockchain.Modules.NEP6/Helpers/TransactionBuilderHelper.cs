﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using POMABlockchain.Modules.Core;
using POMABlockchain.Modules.KeyPairs;
using POMABlockchain.Modules.NEP6.Transactions;
using POMABlockchain.Modules.Rest.DTOs.POMABlockchainScan;
using POMABlockchain.Modules.Rest.Interfaces;
using POMABlockchain.Modules.Rest.Services;
using Helper = POMABlockchain.Modules.KeyPairs.Helper;

namespace POMABlockchain.Modules.NEP6.Helpers
{
    public static class TransactionBuilderHelper
    {
        public static readonly Dictionary<string, string> SystemAssets = new Dictionary<string, string>
        {
            {"POMABlockchain", Utils.POMABlockchainToken},
            {"GAS", Utils.GasToken},
        };

        private static Fixed8 SystemFee => Fixed8.Zero;

        public static async Task<Dictionary<string, List<Unspent>>> GetUnspent(string address,
            IPOMABlockchainscanService restService)
        {
            if (restService == null) throw new NullReferenceException("REST client not configured");
            var addressBalance = await restService.GetBalanceAsync(address);

            var result = new Dictionary<string, List<Unspent>>();
            if (addressBalance.Balance != null)
                foreach (var balanceEntry in addressBalance.Balance)
                {
                    var child = balanceEntry.Unspent;
                    if (!(child?.Count > 0)) continue;
                    List<Unspent> list;
                    if (result.ContainsKey(balanceEntry.Asset))
                    {
                        list = result[balanceEntry.Asset];
                    }
                    else
                    {
                        list = new List<Unspent>();
                        result[balanceEntry.Asset] = list;
                    }

                    list.AddRange(balanceEntry.Unspent.Select(data => new Unspent
                    {
                        TxId = data.TxId,
                        N = data.N,
                        Value = data.Value
                    }));
                }

            return result;
        }

        // Old method
        public static async Task<(List<SignedTransaction.Input> inputs, List<SignedTransaction.Output> outputs)>
           GenerateInputsOutputs(KeyPair key, string symbol, IEnumerable<TransferOutput> targets, decimal fee,
               IPOMABlockchainscanService restService)
        {
            var address = Helper.CreateSignatureRedeemScript(key.PublicKey);
            var unspent = await GetUnspent(address.ToScriptHash().ToAddress(), restService);

            // filter any asset lists with zero unspent inputs
            unspent = unspent.Where(pair => pair.Value.Count > 0).ToDictionary(pair => pair.Key, pair => pair.Value);

            var inputs = new List<SignedTransaction.Input>();
            var outputs = new List<SignedTransaction.Output>();

            string assetId;

            var info = GetAssetsInfo();
            if (info.ContainsKey(symbol))
                assetId = info[symbol];
            else
                throw new WalletException($"{symbol} is not a valid blockchain asset.");

            decimal cost = 0;

            var fromHash = key.PublicKeyHash.ToArray();
            List<TransferOutput> transactionOutputs = null;
            if (targets != null)
            {
                transactionOutputs = targets.ToList();
                foreach (var target in transactionOutputs)
                {
                    if (target.AddressHash.SequenceEqual(fromHash))
                        throw new WalletException("Target can't be same as input");

                    cost += target.Amount;
                }
            }

            if (unspent.Count > 0)
            {
                var sources = unspent[symbol];
                decimal selected = 0;

                foreach (var src in sources)
                {
                    selected += (decimal)src.Value;

                    var input = new SignedTransaction.Input
                    {
                        PrevHash = src.TxId.HexToBytes().Reverse().ToArray(),
                        PrevIndex = src.N
                    };

                    inputs.Add(input);

                    if (selected >= cost) break;
                }

                if (selected < cost) throw new WalletException($"Not enough {symbol}");

                if (cost > 0 && targets != null)
                    foreach (var target in transactionOutputs)
                    {
                        var output = new SignedTransaction.Output
                        {
                            AssetId = assetId.HexToBytes().Reverse().ToArray(),
                            ScriptHash = target.AddressHash.ToArray(),
                            Value = target.Amount
                        };
                        outputs.Add(output);
                    }

                if (selected > cost || cost == 0)
                {
                    var left = selected - cost;
                    var signatureScript = Helper.CreateSignatureRedeemScript(key.PublicKey).ToScriptHash();
                    var change = new SignedTransaction.Output
                    {
                        AssetId = assetId.HexToBytes().Reverse().ToArray(),
                        ScriptHash = signatureScript.ToArray(),
                        Value = left
                    };
                    outputs.Add(change);
                }
            }

            return (inputs, outputs);
        }

        public static async Task<(List<ClaimableElement>, decimal amount)> GetClaimable(string address, IPOMABlockchainscanService restService)
        {
            var claimable = await restService.GetClaimableAsync(address);
            var amount = claimable.Unclaimed;

            return (claimable.ClaimableList.ToList(), (decimal)amount);
        }

        public static List<TransferOutput> BuildTransferOutputs(string address, Dictionary<string, decimal> symbolsAndAmount)
        {
            var targets = new List<TransferOutput>();
            var toScriptHash = address.ToScriptHash().ToArray();
            foreach (var symbol in symbolsAndAmount.Keys)
            {
                var target = new TransferOutput
                {
                    AssetId = SystemAssets[symbol].HexToBytes().Reverse().ToArray(),
                    AddressHash = toScriptHash,
                    Amount = symbolsAndAmount[symbol],
                    Symbol = symbol
                };
                targets.Add(target);
            }
            return targets;
        }

        internal static Dictionary<string, string> GetAssetsInfo()
        {
            if (SystemAssets == null)
            {
                AddAsset("POMABlockchain", Utils.POMABlockchainToken);
                AddAsset("GAS", Utils.GasToken);
            }

            return SystemAssets;
        }

        private static void AddAsset(string symbol, string hash)
        {
            SystemAssets[symbol] = hash;
        }
    }
}