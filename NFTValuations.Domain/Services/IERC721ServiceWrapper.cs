using Nethereum.RPC.Eth.DTOs;
using NFTValuations.Contracts.ERC721.ContractDefinition;
using System.Numerics;
using System.Threading.Tasks;

namespace NFTValuations.Domain.Services
{
    public interface IERC721ServiceWrapper
    {
        public Task<string> TokenURIQueryAsync(TokenURIFunction tokenURIFunction, BlockParameter blockParameter = null);

        public Task<string> TokenURIQueryAsync(BigInteger tokenId, BlockParameter blockParameter = null);
    }
}
