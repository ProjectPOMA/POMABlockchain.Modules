using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.KeyPairs;
using POMABlockchain.Modules.NEP6;
using POMABlockchain.Modules.NEP6.Transactions;
using POMABlockchain.Modules.Rest.Services;
using POMABlockchain.Modules.RPC.Services;
using POMABlockchain.Modules.RPC;
using Newtonsoft.Json;

namespace POMABlockchain.Modules.Demo
{
    public class Program
    {
        private static readonly RpcClient RpcClient = new RpcClient(new Uri("http://seed3.aphelion-POMABlockchain.com:10332"));
        private static readonly RpcClient RpcTestNetClient = new RpcClient(new Uri("http://test5.cityofzion.io:8880"));

        public static void Main(string[] args)
        {
            try
            {
                //https://api.happynodes.f27.ventures
                HappyNodesService().Wait();

                var POMABlockchainApiCompleteService = SetupCompletePOMABlockchainService();

                var POMABlockchainApiSimpleContractService = SetupSimpleService();
                var POMABlockchainApiSimpleAccountService = SetupAnotherSimpleService();
                //You can also create a custom service with only the stuff that you need by creating a class that implements(":") RpcClientWrapper like: public class CustomService : RpcClientWrapper

                var nep5ApiService = SetupNep5Service();

                BlockApiTest(POMABlockchainApiCompleteService).Wait();

                TestNep5Service(nep5ApiService).Wait();

                //create rest api client
                RestClientTest().Wait();

                //nodes list from http://monitor.cityofzion.io/
                NodesListTestAsync().Wait();

                //https://n1.cityofzion.io/v1/"
                NotificationsService().Wait();



                WalletAndTransactionsTest().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception: {ex.Message}");
            }
        }

        // Returns a POMABlockchainApiService that has all the api calls (except the nep5)
        private static POMABlockchainApiService SetupCompletePOMABlockchainService()
        {
            return new POMABlockchainApiService(RpcClient);
        }

        // Returns an instance of ContractService with only the api calls that concern contracts
        private static POMABlockchainApiContractService SetupSimpleService()
        {
            return new POMABlockchainApiContractService(RpcClient);
        }

        // Another example of a simple Service - Account api calls
        private static POMABlockchainApiAccountService SetupAnotherSimpleService()
        {
            return new POMABlockchainApiAccountService(RpcClient);
        }

        // Returns an instance of POMABlockchainNep5Service with the api calls that concern nep5 tokens.
        private static POMABlockchainNep5Service SetupNep5Service()
        {
            return new POMABlockchainNep5Service(RpcClient); //TNC token script hash
        }

        // Block api demonstration
        private static async Task BlockApiTest(POMABlockchainApiService service)
        {
            RPC.DTOs.Block
                genesisBlock =
                    await service.Blocks.GetBlock
                        .SendRequestAsync(
                            0); // Get genesis block by index (can pass a string with block hash as parameter too)
            string bestBlockHash = await service.Blocks.GetBestBlockHash.SendRequestAsync();
            int blockCount = await service.Blocks.GetBlockCount.SendRequestAsync();
            string blockHash = await service.Blocks.GetBlockHash.SendRequestAsync(0);
            string serializedBlock =
                await service.Blocks.GetBlockSerialized
                    .SendRequestAsync(0); // (can pass a string with block hash as parameter too)
            string blockFee = await service.Blocks.GetBlockSysFee.SendRequestAsync(0);
        }

        // Nep5 api demonstration
        private static async Task TestNep5Service(POMABlockchainNep5Service nep5Service)
        {
            var name = await nep5Service.GetName("ed07cffad18f1308db51920d99a2af60ac66a7b3", true);
            var decimals = await nep5Service.GetDecimals("ed07cffad18f1308db51920d99a2af60ac66a7b3");
            var totalsupply = await nep5Service.GetTotalSupply("ed07cffad18f1308db51920d99a2af60ac66a7b3", 8);
            var symbol = await nep5Service.GetSymbol("ed07cffad18f1308db51920d99a2af60ac66a7b3", true);
            var balance = await nep5Service.GetBalance("ed07cffad18f1308db51920d99a2af60ac66a7b3",
                "0x3640b023405b4b9c818e8387bd01f67bba04dad2", 8);

            Debug.WriteLine(
                $"Token info: \nName: {name} \nSymbol: {symbol} \nDecimals: {decimals} \nTotalSupply: {totalsupply} \nBalance: {balance}");
        }


        private static async Task RestClientTest()
        {
            var testAddress = "AJN2SZJuF7j4mvKaMYAY9N8KsyD4j1fNdf";

            var restService = new POMABlockchainScanRestService(POMABlockchainScanNet.MainNet); // service creation

            // api calls
            var getBalance = await restService.GetBalanceAsync(testAddress);
            var getClaimed = await restService.GetClaimedAsync(testAddress);
            var getClaimable = await restService.GetClaimableAsync(testAddress);
            var getUnclaimed = await restService.GetUnclaimedAsync(testAddress);
            // var getAddress = await restService.GetAddressAsync(testAddress);
            var nodes = await restService.GetAllNodesAsync();
            var transaction =
                await restService.GetTransactionAsync(
                    "610e2a4c7cdc4f311be65cee48e076871b685e13cc750397f4ee5f800da3309a");
            var height = await restService.GetHeight();
            var abstractAddress = await restService.GetAddressAbstracts("AGbj6WKPUWHze12zRyEL5sx8nGPVN6NXUn", 0);
            var addressToAddressAbstract = await restService.GetAddressToAddressAbstract(
                "AJ5UVvBoz3Nc371Zq11UV6C2maMtRBvTJK",
                "AZCcft1uYtmZXxzHPr5tY7L6M85zG7Dsrv", 0);
            var block = await restService.GetBlock("54ffd56d6a052567c5d9abae43cc0504ccb8c1efe817c2843d154590f0b572f7");
            var lastTransactionsByAddress =
                await restService.GetLastTransactionsByAddress("AGbj6WKPUWHze12zRyEL5sx8nGPVN6NXUn", 0);
        }

        // Test getting the nodes list registered on http://monitor.cityofzion.io
        private static async Task<NodeList> NodesListTestAsync()
        {
            var service = new POMABlockchainNodesListService();
            var result = await service.GetNodesList(MonitorNet.TestNet);
            var nodes = JsonConvert.DeserializeObject<NodeList>(result);
            return nodes;
        }

        private static async Task WalletAndTransactionsTest()
        {
            // Create online wallet and import account
            var walletManager = new WalletManager(new POMABlockchainScanRestService(POMABlockchainScanNet.MainNet), RpcClient);
            var importedAccount = await walletManager.ImportAccount("*** ENCRYPTED KEY***", "*** PASSWORD***", "Test");

            // Get account signer for transactions
            if (importedAccount.TransactionManager is AccountSignerTransactionManager accountSignerTransactionManager)
            {
                // Send native assets
                var assets = new Dictionary<string, decimal> { { "POMABlockchain", 1 }, { "GAS", 1 } };
                var sendPOMABlockchainAndGasTx =
                    await accountSignerTransactionManager.SendAsset("** INSERT TO ADDRESS HERE **", assets);

                // Call contract
                var scriptHash = "** INSERT CONTRACT SCRIPTHASH **".ToScriptHash().ToArray();
                var operation = "balanceOf";
                var arguments = new object[] { "arg1" };

                var contractCallTx =
                    await accountSignerTransactionManager.CallContract(scriptHash, operation, arguments);

                // Estimate Gas consumed from contract call
                var estimateContractGasCall =
                    await accountSignerTransactionManager.EstimateGasContractCall(scriptHash, operation, arguments);

                // Call contract with attached assets
                var assetToAttach = "GAS";
                var output = new List<TransferOutput>()
                {
                    new TransferOutput()
                    {
                        AddressHash = "** INSERT TO ADDRESS HERE**".ToScriptHash().ToArray(),
                        Amount = 2,
                    }
                };
                var contractCallWithAttachedTx =
                    await accountSignerTransactionManager.CallContract(scriptHash, operation, arguments, assetToAttach,
                        output);

                // Claim gas
                var callGasTx = await accountSignerTransactionManager.ClaimGas();

                // Transfer NEP5 tokens
                var transferNepTx =
                    await accountSignerTransactionManager.TransferNep5("** INSERT TO ADDRESS HERE**", 32.3m,
                        scriptHash);

                // Confirm a transaction
                var confirmedTransaction =
                    await accountSignerTransactionManager.WaitForTxConfirmation(transferNepTx.Hash.ToString());
            }
        }

        private static async Task NotificationsService()
        {
            var notificationService = new NotificationsService();
            var addressNotifications = await notificationService.GetAddressNotifications("AGfGWQeM6md6RtEsTUivQNXhp8p4ytkDMR", 1, null, 13, 2691913, 500);
            var blockNotifications = await notificationService.GetBlockNotifications(10);
            var contractNotifications =
                await notificationService.GetContractNotifications("0x67a5086bac196b67d5fd20745b0dc9db4d2930ed");
            var tokenList = await notificationService.GetTokens();
            var transaction =
                await notificationService.GetTransactionNotifications(
                    "1db9ad15febbf7b9cfa11603ecbe52aad5be6137a9e1d29e83465fa664c2a6ed");
        }

        private static async Task HappyNodesService()
        {
            var happyNodesService = new HappyNodesService();
            var bestBlock = await happyNodesService.GetBestBlock();
            var lastBlock = await happyNodesService.GetLastBlock();
            var blockTime = await happyNodesService.GetBlockTime();

            var unconfirmedTxs = await happyNodesService.GetUnconfirmed();

            var nodesFlat = await happyNodesService.GetNodesFlat();
            var nodes = await happyNodesService.GetNodes();
            var nodeById = await happyNodesService.GetNodeById(482);
            var edges = await happyNodesService.GetEdges();
            var nodesList = await happyNodesService.GetNodesList();

            var dailyHistory = await happyNodesService.GetDailyNodeHistory();
            var weeklyHistory = await happyNodesService.GetWeeklyNodeHistory();

            var dailyStability = await happyNodesService.GetDailyNodeStability(480);
            var weeklyStability = await happyNodesService.GetWeeklyNodeStability(0);

            var dailyLatency = await happyNodesService.GetDailyNodeLatency(480);
            var weeklyLatency = await happyNodesService.GetWeeklyNodeLatency(480);
            var blockHeightLag = await happyNodesService.GetNodeBlockheightLag(0);
            var endpoints = await happyNodesService.GetEndPoints();
        }

    }
}