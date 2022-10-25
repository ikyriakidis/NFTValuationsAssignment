using System.Net.Http;
using System.Threading.Tasks;
using NFTValuations.Domain.Models;

namespace NFTValuations.Domain.Handlers
{
    public class IPFSHandler : HttpHandler
    {

        public IPFSHandler(HttpClient httpClient) : base(httpClient) { }

        public override async Task<ResponseDto> Handle(string uri)
        {
            uri = $"https://ipfs.io/ipfs/{uri.Replace("ipfs://", string.Empty)}";
            return await HandleHttpRequestAsync(uri);
        }
    }
}