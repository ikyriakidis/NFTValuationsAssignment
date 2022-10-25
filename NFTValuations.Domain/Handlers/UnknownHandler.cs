using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NFTValuations.Domain.Models;

namespace NFTValuations.Domain.Handlers
{
  public class UnknownHandler : IHandler
  {
    private readonly ILogger<IHandler> _logger;

    public UnknownHandler(ILogger<IHandler> logger)
    {
      _logger = logger;
    }

    public async Task<ResponseDto> Handle(string uri)
    {
      _logger.LogInformation($"Request for unknown protocol: {uri}");
      return await Task.FromResult(new ResponseDto());
    }
  }
}