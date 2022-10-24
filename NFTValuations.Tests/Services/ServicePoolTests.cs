using FluentAssertions;
using Nethereum.Web3;
using NFTValuations.Domain.Services;
using Xunit;

namespace NFTValuations.Tests.Services
{
    public sealed class ServicePoolTests
    {
        [Fact]
        public void GetserviceInstance_ReturnsClientWithCorrectContractAddress() 
        {
            // Arrange
            var contractAddress = "testContractAddress";
            var web3 = new Web3();
            
            var sut = new ServicePool(web3);

            // Act
            ERC721ServiceWrapper serviceInstance = sut.GetserviceInstance(contractAddress);

            // Assert
            serviceInstance.ContractAddress.Should().Be(contractAddress);
        }
    }
}
