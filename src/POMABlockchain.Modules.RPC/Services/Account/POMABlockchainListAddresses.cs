﻿using System.Threading.Tasks;
using POMABlockchain.Modules.JsonRpc.Client;
using POMABlockchain.Modules.RPC.DTOs;

namespace POMABlockchain.Modules.RPC.Services.Account
{
    /// <summary>
    ///     listaddress 
    ///     Lists all the addresses in the current wallet
    /// 
    ///     Parameters
    ///     None
    /// 
    ///     Returns
    ///     address：The address in the wallet.
    ///     watchonly： Indicates whether it is a watch only address. 
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"listaddress","params":[],"id":1}'
    /// 
    ///     Result
    ///     {
    ///     "jsonrpc": "2.0",
    ///     "id": 1,
    ///      "result": [
    ///    {
    ///        "address": "ASL3KCvJasA7QzpYGePp25pWuQCj4dd9Sy",
    ///        "haskey": true,
    ///        "label": null,
    ///        "watchonly": false
    ///    },
    ///    {
    ///        "address": "AV2Ai7PXcNbjTSeKgWqsDEjLaEAJZpytru",
    ///        "haskey": true,
    ///        "label": null,
    ///       "watchonly": false
    ///    }
    ///    ]
    /// }
    /// </summary>
    public class POMABlockchainListAddresses : RpcRequestResponseHandler<WalletAddress[]>
    {
        public POMABlockchainListAddresses(IClient client) : base(client, ApiMethods.listaddress.ToString())
        {
        }

        public Task<WalletAddress[]> SendRequestAsync(object id = null)
        {
            return base.SendRequestAsync(id);
        }
    }
}