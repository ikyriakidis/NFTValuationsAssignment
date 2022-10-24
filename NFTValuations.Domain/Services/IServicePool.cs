namespace NFTValuations.Domain.Services
{
    public interface IServicePool
    {
        public ERC721ServiceWrapper GetserviceInstance(string contractAddress);
    }
}
