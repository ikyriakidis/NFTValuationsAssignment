using System.Threading.Tasks;
using NFTValuations.Domain.Models;

namespace NFTValuations.Domain.Handlers
{
  public interface IHandler
  {
    Task<ResponseDto> Handle(string uri);
  }
}