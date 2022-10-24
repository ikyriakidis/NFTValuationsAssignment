using System.Collections.Concurrent;
using Nethereum.Web3;

namespace NFTValuations.Domain.Services
{
    public sealed class ServicePool : IServicePool
    {
        private readonly Web3 _web3;
        private ConcurrentDictionary<string, ERC721ServiceWrapper> _serviceInstances;
        public ServicePool(IWeb3 web3)
        {
            _web3 = (Web3)web3;
            _serviceInstances = new ConcurrentDictionary<string, ERC721ServiceWrapper>();
        }

        public ERC721ServiceWrapper GetserviceInstance(string contractAddress)
        {
            if (_serviceInstances.ContainsKey(contractAddress))
            {
                return _serviceInstances[contractAddress];
            }
            else
            {
                var newServiceInstance = new ERC721ServiceWrapper(_web3, contractAddress);
                _serviceInstances.TryAdd(contractAddress, newServiceInstance);
                return newServiceInstance;
            }
        }
    }
}
