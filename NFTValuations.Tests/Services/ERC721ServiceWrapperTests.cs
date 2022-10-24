using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Nethereum.Web3;
using NFTValuations.Domain.Services;
using System.Threading.Tasks;
using Xunit;

namespace NFTValuations.Tests.Services
{
    public sealed class ERC721ServiceWrapperTests
    {
        [Theory]
        [InlineData("0x1a92f7381b9f03921564a437210bb9396471050c", 0, "https://api.coolcatsnft.com/cat/0")]
        [InlineData("0xec9c519d49856fd2f8133a0741b4dbe002ce211b", 30, "https://ipfs.io/ipfs/QmVtJUJrjWfthrGFvkwFciZcvoNLzCrC6EXAQwimYBUZeQ/30")]
        [InlineData("0xbc4ca0eda7647a8ab7c2061c2e118a18a936f13d", 1234, "ipfs://QmeSjSinHpPnmXmspMjwiXyN6zS4E9zccariGR3jxcaWtq/1234")]
        public async Task TokenURIQueryAsync_ReturnsCorrectData_WhenInputIsValid(
            string contractAddress, int tokenId, string expectedResult)
        {
            // Arrange
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var infuraIoToken = (config["AppSettings:InfuraIoToken"]);

            var web3 = new Web3($"https://mainnet.infura.io/v3/{infuraIoToken}");

            var sut = new ERC721ServiceWrapper(web3, contractAddress);

            // Act
            var result = await sut.TokenURIQueryAsync(tokenId);

            // Assert
            result.Should().Be(expectedResult);
        }   
    }
}
