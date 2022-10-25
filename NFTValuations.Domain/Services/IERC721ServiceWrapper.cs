using System.Numerics;
using System.Threading.Tasks;

namespace NFTValuations.Domain.Services
{
    public interface IERC721ServiceWrapper
    {
        public Task<string> TokenURIQueryAsync(BigInteger tokenId);
    }
}
