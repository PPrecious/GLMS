using GLMS.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;
using Xunit;
using System.Threading.Tasks;

namespace GLMS.Tests
{
    public class FileServiceTests
    {
        [Fact]
        public async Task SavePdfAsync_ShouldRejectNonPdfFile()
        {
            var envMock = new Mock<IWebHostEnvironment>();
            envMock.Setup(e => e.WebRootPath).Returns(Path.GetTempPath());

            var service = new FileService(envMock.Object);

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("test.exe");
            fileMock.Setup(f => f.Length).Returns(100);

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await service.SavePdfAsync(fileMock.Object);
            });
        }
    }
}