using RozmieniarkaApp.Services;
using RozmieniarkaApp.Enums;
namespace RozmieniarkaAppTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1Async()
        {
            string status = await DownloadDataService.DownloadStatus(DataQueryType.Status);
            Assert.Equal("0010160002800003", status);
            status = await DownloadDataService.DownloadStatus(DataQueryType.Inserted);
            Assert.Equal("003036PLN0200001PLN0100003PLN0500001", status);
            status = await DownloadDataService.DownloadStatus(DataQueryType.Withdrawn);
            Assert.Equal("002036PLM0010013PLM0020016PLM0050011", status);
        }
        [Fact]
        public void Test2()
        {
            Assert.Equal(1, 1);
        }
    }
}