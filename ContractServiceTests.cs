using GLMS.Web.Services;
using Xunit;

namespace GLMS.Tests
{
    public class ContractServiceTests
    {
        [Fact]
        public void IsContractValid_ShouldReturnFalse_WhenExpired()
        {
            var service = new ContractService();

            var result = service.IsContractValid("Expired");

            Assert.False(result);
        }

        [Fact]
        public void IsContractValid_ShouldReturnTrue_WhenActive()
        {
            var service = new ContractService();

            var result = service.IsContractValid("Active");

            Assert.True(result);
        }
    }
}