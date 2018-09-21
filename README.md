<p>Builds </p>

<img class="status-badge-image" src="https://projectpoma.visualstudio.com/POMABlockchain.Modules/_apis/build/status/POMABlockchain.Modules-ASP.NET%20Core-CI" alt="status badge">

<hr />
<p align="center">
  <img
    src="http://res.cloudinary.com/vidsy/image/upload/v1503160820/CoZ_Icon_DARKBLUE_200x178px_oq0gxm.png"
    width="125px;">
</p>

<h1 align="center">POMABlockchain Modules</h1>

<p align="center">
  Modular packages for C# devs to use on your <b>POMA</b> blockchain project.
</p>

<p align="center">
  <b>Waiting for peer review. Use on main net at your own risk</b>
</p>


## Libraries (ready for use)

|  Project Source | Nuget Package |  Description |
| ------------- |--------------------------|-----------|
| [POMABlockchainModules.RPC](https://github.com/CityOfZion/POMABlockchainModules/tree/master/src/POMABlockchainModules.RPC)    | [![NuGet version](https://img.shields.io/badge/nuget-1.0.10-green.svg)](https://www.nuget.org/packages/POMABlockchainModules.RPC/)| RPC Class Library to interact with POMABlockchain RPC nodes |
| [POMABlockchainModules.JsonRpc.Client](https://github.com/CityOfZion/POMABlockchainModules/tree/master/src/POMABlockchainModules.JsonRpc.Client) | [![NuGet version](https://img.shields.io/badge/nuget-1.0.2-green.svg)](https://www.nuget.org/packages/POMABlockchainModules.JsonRpc.Client/)| Base RPC client definition, used in POMABlockchainModules.RPC|
| [POMABlockchainModules.Rest](https://github.com/CityOfZion/POMABlockchainModules/tree/master/src/POMABlockchainModules.Rest)    | [![NuGet version](https://img.shields.io/badge/nuget-1.0.7-green.svg)](https://www.nuget.org/packages/POMABlockchainModules.Rest/)| Simple Rest client for https://POMABlockchainscan.io public API |

## Libraries (in dev)
|  Project Source | Nuget Package |  Description |
| ------------- |--------------------------|-----------|
| [POMABlockchainModules.Core](https://github.com/CityOfZion/POMABlockchainModules/tree/feature-core/src/POMABlockchainModules.Core)    | [![NuGet version](https://img.shields.io/badge/nuget-0.0.3-yellow.svg)](https://www.nuget.org/packages/POMABlockchainModules.Core/)| Core data types and methods used in POMABlockchainModules |
| [POMABlockchainModules.NVM](https://github.com/CityOfZion/POMABlockchainModules/tree/feature-core/src/POMABlockchainModules.NVM)    | [![NuGet version](https://img.shields.io/badge/nuget-0.0.3-yellow.svg)](https://www.nuget.org/packages/POMABlockchainModules.NVM/)| POMABlockchain VM with only the necessary functions to support script construction and KeyPair/NEP6 |
| [POMABlockchainModules.KeyPairs](https://github.com/CityOfZion/POMABlockchainModules/tree/feature-core/src/POMABlockchainModules.KeyPairs)    | [![NuGet version](https://img.shields.io/badge/nuget-0.0.5-yellow.svg)](https://www.nuget.org/packages/POMABlockchainModules.KeyPairs/)| KeyPair project, has the crypto methods needed for KeyPair creation and KeyPair definition |
| [POMABlockchainModules.NEP6](https://github.com/CityOfZion/POMABlockchainModules/tree/feature-core/src/POMABlockchainModules.NEP6)    | [![NuGet version](https://img.shields.io/badge/nuget-0.0.16-yellow.svg)](https://www.nuget.org/packages/POMABlockchainModules.NEP6/)| NEP6 light wallet implementation |


## RPC client

Develop with decoupling in mind to make maintenance and new RPC methods implemented more quickly:

* Client base and RPC client implementation - POMABlockchainModules.JsonRpc.Client project
* DTO'S, Services, Helpers - POMABlockchainModules.RPC (main project)
* Tests - POMABlockchainModules.RPC.Tests
* Demo - Simple demonstration project

Setup the rpc client node

```C#
var rpcClient = new RpcClient(new Uri("http://seed5.POMABlockchain.org:10332"));
var POMABlockchainApiService = new POMABlockchainApiService(rpcClient);
```

With **POMABlockchainApiService** you have all the methods available, organized by:
Accounts,
Assets,
Block,
Contract,
Node,
Transaction

Then you just need to choose the wanted service, call ```SendRequestAsync()``` and pass the necessary parameters if needed.
e.g.

```C#
var accountsService = POMABlockchainApiService.Accounts;
var state = accountsService.GetAccountState.SendRequestAsync("ADDRESS HERE");
```

If you don't need all the services, you can simply create an instance of the desired service.

```C#
var blockService = new POMABlockchainApiBlockService(new RpcClient(new Uri("http://seed5.POMABlockchain.org:10332")));
var bestBlockHash  = await blockService.GetBestBlockHash.SendRequestAsync();
```

All rpc calls return a DTO or a simple type like string or int.

### NEP 5 service
You can also create a service to query NEP5 tokens. 

```C#
var scriptHash = "ed07cffad18f1308db51920d99a2af60ac66a7b3";
var nep5Service = POMABlockchainNep5Service(new RpcClient(new Uri("http://seed2.aphelion-POMABlockchain.com:10332")));
var name = await nep5Service.GetName(scriptHash, true);
var decimals = await nep5Service.GetDecimals(scriptHash);
var totalsupply = await nep5Service.GetTotalSupply(scriptHash, 8);
var symbol = await nep5Service.GetSymbol(scriptHash, true);
var balance = await nep5Service.GetBalance(scriptHash, "0x3640b023405b4b9c818e8387bd01f67bba04dad2", 8);
Debug.WriteLine($"Token info: \nName: {name} \nSymbol: {symbol} \nDecimals: {decimals} \nTotalSupply: {totalsupply} \nBalance: {balance}");
                
Token info: 
Name: Trinity Network Credit 
Symbol: TNC 
Decimals: 8 
TotalSupply: 1000000000 
Balance: 1457.82
```

## Rest services
### Create Rest service (only POMABlockchainscan available right now)

```C# 
var restService = new POMABlockchainScanRestService(POMABlockchainScanNet.MainNet);
```
or use your own local POMABlockchainScan

```C# 
var restService = new POMABlockchainScanRestService("https://url.here/api/main_net[or test_net]/v1/");
```

### Using the API

```C# 
var transaction_json = await restService.GetTransactionAsync("599dec5897d416e9a668e7a34c073832fe69ad01d885577ed841eec52c1c52cf");
```
This returns the json in string format. But you can also transform the result to a defined DTO for easier use:

```C# 
var transaction_json = await restService.GetTransactionAsync("599dec5897d416e9a668e7a34c073832fe69ad01d885577ed841eec52c1c52cf");
var transactionDto = Transaction.FromJson(transaction_json);
```
You can see all the available calls in Demo console project.

## Nodes list
Besides the "get_all_nodes" call on POMABlockchainScan API, there also an option to use http://monitor.cityofzion.io/ to get all the nodes with some extra info.

```C# 
var service = new POMABlockchainNodesListService();
var result = await service.GetNodesList(MonitorNet.TestNet);
var nodes = JsonConvert.DeserializeObject<NodeList>(result);
```

## NEP6 Compatible Wallet
The wallet creation is of WalletManager.cs responsability. You can use this online or offline, it only depends on the initialization.
The online wallet needs a rest and a rpc client:

```C# 
public WalletManager(IPOMABlockchainRestService restService, IClient client, Wallet wallet = null)
```

### Create an empty wallet and import and account 
You can use wif in string or byte format
```C# 
var walletManager = new WalletManager(new POMABlockchainScanRestService(POMABlockchainScanNet.MainNet), RpcClient);
var importedAccount = walletManager.ImportAccount("** INSERT WIF HERE **", "Custom account label");
```
Or use NEP6 (this one uses async/await because it can be a heavy operation, especially on mobile hardware)
```C# 
var importedAccount = await walletManager.ImportAccount("** INSERT NEP6 PASSPHRASE HERE **", "** PASSWORD **", "Custom account label");
```

## Transactions
For now, you need to use this check before using the TransactionManager, responsable for the making and signing the transactions. This is needed because POMABlockchainModules will use different signing strategies, but for now, only the AccountSignerTransactionManager is available.

### Sending native assets
```C# 
if (importedAccount.TransactionManager is AccountSignerTransactionManager accountSignerTransactionManager)
{
    var sendGasTx = await accountSignerTransactionManager.SendAsset("** INSERT TO ADDRESS HERE **", "GAS", 323.032m);
    var sendPOMABlockchainT = await accountSignerTransactionManager.SendAsset("** INSERT TO ADDRESS HERE **", "POMABlockchain", 13m) 
}
```
### Making a contract call
```C# 
var scriptHash = "** INSERT CONTRACT SCRIPTHASH **".ToScriptHash().ToArray();
var operation = "balanceOf";
var arguments = new object[] { "arg1" };        

var contractCallTx = await accountSignerTransactionManager.CallContract(scriptHash, operation, arguments);
```

### Estimate contract call gas consumption
```C#
var estimateContractGasCall = await accountSignerTransactionManager.EstimateGasContractCall(scriptHash, operation, arguments);
```

### Making a call contract with attached assets
```C#
var assetToAttach = "GAS";
                var output = new List<TransactionOutput>()
                {
                    new TransactionOutput()
                    {
                        AddressHash = "** INSERT TO ADDRESS HERE**".ToScriptHash().ToArray(),
                        Amount = 2,
                    }
                };
var contractCallWithAttachedTx =
                    await accountSignerTransactionManager.CallContract(scriptHash, operation, arguments, assetToAttach,
                        output);
```

### Claiming Gas
```C#
var callGasTx = await accountSignerTransactionManager.ClaimGas();
```

### Transfer NEP5 tokens
```C#
var transferNepTx = await accountSignerTransactionManager.TransferNep5("** INSERT TO ADDRESS HERE**", 32.3m, scriptHash);
```


## Contributing


## Authors

* **Bruno Freitas** - [BrunoFreitasgit](https://github.com/BrunoFreitasgit)
* [POMABlockchain](https://github.com/POMABlockchain-project/)
* [POMABlockchain-lux](https://github.com/CityOfZion/POMABlockchain-lux)
* Multiple base code is taken/inspired from [Nethereum](https://github.com/Nethereum/Nethereum) project

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/BrunoFreitasgit/POMABlockchain-RPC-SharpClient/blob/master/LICENSE) file for details
