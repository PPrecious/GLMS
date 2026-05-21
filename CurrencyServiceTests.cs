using Moq;
using System.Net.Http;
using Xunit;
using GLMS.Web.Services;

public class CurrencyServiceTests
{
    [Fact]
    public void ConvertUsdToZar_ShouldReturnCorrectValue()
    {
        var httpClient = new HttpClient();
        var service = new CurrencyService(httpClient);

        decimal result = service.ConvertUsdToZar(100, 18.5m);

        Assert.Equal(1850m, result);
    }
}