using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using NFTValuations.Domain.Handlers;

namespace NFTValuations.Domain.Services
{
    public class Detector: IDetector
    {
        private readonly Dictionary<string, IHandler> _handlers = new Dictionary<string, IHandler>();

        public Detector(ILogger<IHandler> logger, HttpClient httpClient)
        {
            _handlers["ipfs"] = new IPFSHandler(httpClient);
            _handlers["http"] = new HttpHandler(httpClient);
            _handlers["unknown"] = new UnknownHandler(logger);
        }

        public IHandler Detect(string token)
        {
            if (token.StartsWith("ipfs://"))
            {
                return _handlers["ipfs"];
            }

            if (token.StartsWith("http://") || token.StartsWith("https://"))
            {
                return _handlers["http"];
            }

            return _handlers["unknown"];
        }
    }
}