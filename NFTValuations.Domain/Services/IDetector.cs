using NFTValuations.Domain.Handlers;

namespace NFTValuations.Domain.Services
{
    public interface IDetector
    {
        public IHandler Detect(string token);
    }
}
