using Nethereum.Web3;
using NFTValuations.Contracts.ERC721;
using System.Numerics;
using System.Threading.Tasks;

namespace NFTValuations.Domain.Services
{
    public class ERC721ServiceWrapper: IERC721ServiceWrapper
    {
        public string ContractAddress => _eRC721Service?.ContractHandler?.ContractAddress??string.Empty;
        private readonly ERC721Service _eRC721Service;

        public ERC721ServiceWrapper(IWeb3 web3, string contractAddress)
        {
            _eRC721Service = new ERC721Service((Web3)web3, contractAddress);
        }

        public Task<string> TokenURIQueryAsync(BigInteger tokenId)
        {
            return _eRC721Service.TokenURIQueryAsync(tokenId, null);
        }
    }
}
