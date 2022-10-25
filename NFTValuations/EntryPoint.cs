using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NFTValuations.Domain.Services;
using NFTValuations.Contracts.ERC721.ContractDefinition;

namespace NFTValuations
{
    public class EntryPoint
    {
        private readonly ILogger<EntryPoint> _logger;
        private readonly IServicePool _servicePool;
        private readonly IDetector _detector;

        public EntryPoint(
            IDetector detector,
            ILogger<EntryPoint> logger,
            IServicePool servicePool)
        {
            _logger = logger;
            _servicePool = servicePool;
            _detector = detector;
        }
        public async Task RunAsync(string[] args)
        {
            foreach (var argument in args)
            {
                var contractAddress = argument.Split(",")[0];
                int.TryParse(argument.Split(",")[1], out int tokenId);
                var tokenURIFunction = new TokenURIFunction();
                tokenURIFunction.TokenId = tokenId;

                try
                {
                    var service = _servicePool.GetserviceInstance(contractAddress);
                    var result = await service.TokenURIQueryAsync(tokenURIFunction);
                    _logger.LogInformation(result);

                    var handler = _detector.Detect(result);
                    var ret = await handler.Handle(result);
                    _logger.LogInformation(JsonConvert.SerializeObject(ret));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }

            Console.WriteLine("Press any button to exit");
            Console.ReadLine();
        }
    }
}
