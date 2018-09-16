using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using POMABlockchain.Modules.Rest.DTOs.POMABlockchainScan;
using POMABlockchain.Modules.Rest.Interfaces;
using Newtonsoft.Json.Linq;

namespace POMABlockchain.Modules.Rest.Services
{
    public enum POMABlockchainScanNet
    {
        MainNet,
        TestNet
    }

    public class POMABlockchainScanRestService : IPOMABlockchainscanService
    {
        private static readonly string POMABlockchainScanTestNetUrl = "https://POMABlockchainscan-testnet.io/api/test_net/v1/";
        private static readonly string POMABlockchainScanMainNetUrl = "https://POMABlockchainscan.io/api/main_net/v1/";
        private static readonly string getBalanceUrl = "get_balance/";
        private static readonly string getClaimedUrl = "get_claimed/";
        private static readonly string getClaimableUrl = "get_claimable/";
        private static readonly string getUnclaimedUrl = "get_unclaimed/";
        private static readonly string getAllNodes = "get_all_nodes/";
        private static readonly string getTransaction = "get_transaction/";
        private static readonly string getAddressAbstracts = "get_address_abstracts/";
        private static readonly string getAddressPOMABlockchainn = "get_address_POMABlockchainn/";
        private static readonly string getAddressToAddressAbstracts = "get_address_to_address_abstracts/";
        private static readonly string getAsset = "get_asset/";
        private static readonly string getAssets = "get_assets/";
        private static readonly string getBlock = "get_block/";
        private static readonly string getFeesInRange = "get_fees_in_range/";
        private static readonly string getHeight = "get_height/";
        private static readonly string getHighestBlock = "get_highest_block/";
        private static readonly string getLastBlocks = "get_last_blocks/";
        private static readonly string getLastTransactions = "get_last_transactions/";
        private static readonly string getLastTransactionsByAddress = "get_last_transactions_by_address/";
        private static readonly string getNodes = "get_nodes/";

        private readonly HttpClient _restClient;

        public POMABlockchainScanRestService(POMABlockchainScanNet net)
        {
            _restClient = net == POMABlockchainScanNet.MainNet
                ? new HttpClient { BaseAddress = new Uri(POMABlockchainScanMainNetUrl) }
                : new HttpClient { BaseAddress = new Uri(POMABlockchainScanTestNetUrl) };
        }

        public POMABlockchainScanRestService(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));
            _restClient = new HttpClient { BaseAddress = new Uri(url) };
        }

        // TODO: I can refractor this more, move the 3 lines of each call to a function
        public async Task<AddressBalance> GetBalanceAsync(string address)
        {
            var composedUrl = ComposeUrl(getBalanceUrl, address);
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return AddressBalance.FromJson(data);
        }

        public async Task<Claimable> GetClaimableAsync(string address)
        {
            var composedUrl = ComposeUrl(getClaimableUrl, address);
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return Claimable.FromJson(data);
        }

        public async Task<Claimed> GetClaimedAsync(string address)
        {
            var composedUrl = ComposeUrl(getClaimedUrl, address);
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return Claimed.FromJson(data);
        }

        public async Task<Unclaimed> GetUnclaimedAsync(string address)
        {
            var composedUrl = ComposeUrl(getUnclaimedUrl, address);
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return Unclaimed.FromJson(data);
        }

        public async Task<Transaction> GetTransactionAsync(string hash)
        {
            var composedUrl = ComposeUrl(getTransaction, hash);
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return Transaction.FromJson(data);
        }

        public async Task<List<Node>> GetAllNodesAsync()
        {
            var result = await _restClient.GetAsync(getAllNodes);
            var data = await result.Content.ReadAsStringAsync();
            return Node.FromJson(data).ToList();
        }

        public async Task<long> GetHeight()
        {
            var result = await _restClient.GetAsync(getHeight);
            var data = await result.Content.ReadAsStringAsync();
            return Convert.ToInt64(JObject.Parse(data)["height"].ToString());
        }

        public void ChangeNet(POMABlockchainScanNet net)
        {
            if (_restClient == null) return;
            switch (net)
            {
                case POMABlockchainScanNet.MainNet:
                    _restClient.BaseAddress = new Uri(POMABlockchainScanMainNetUrl);
                    return;
                case POMABlockchainScanNet.TestNet:
                    _restClient.BaseAddress = new Uri(POMABlockchainScanTestNetUrl);
                    return;
            }
        }

        public async Task<AbstractAddress> GetAddressAbstracts(string address, int page = 0)
        {
            var composedUrl = ComposeUrl(getAddressAbstracts, string.Concat(address, "/", page));
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return AbstractAddress.FromJson(data);
        }

        public async Task<AbstractAddress> GetAddressToAddressAbstract(string addressfrom, string addressTo, int page = 0)
        {
            var composedUrl = ComposeUrl(getAddressToAddressAbstracts,
                string.Concat(addressfrom, "/", addressTo, "/", page));
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return AbstractAddress.FromJson(data);
        }

        public async Task<Block> GetBlock(string blockHash)
        {
            var composedUrl = ComposeUrl(getBlock, blockHash);
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return Block.FromJson(data);
        }

        public async Task<Block> GetBlock(int blockHeight)
        {
            var composedUrl = ComposeUrl(getBlock, blockHeight);
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return Block.FromJson(data);
        }

        public async Task<List<Transaction>> GetLastTransactionsByAddress(string address, int page = 0)
        {
            var composedUrl = ComposeUrl(getLastTransactionsByAddress,
                string.Concat(address, "/", page));
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return Transactions.FromJson(data).ToList();
        }

        public async Task<string> GetNodes()
        {
            var result = await _restClient.GetAsync(getNodes);
            var data = await result.Content.ReadAsStringAsync();
            return data;
        }

        private string ComposeUrl(string url, object pathToAdd)
        {
            return $"{url}{pathToAdd}";
        }
    }
}