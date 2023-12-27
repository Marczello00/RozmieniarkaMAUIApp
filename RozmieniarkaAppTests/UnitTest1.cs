using RozmieniarkaApp.Services;
using RozmieniarkaApp.Enums;
namespace RozmieniarkaAppTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            DataQueryType dataQueryType = DataQueryType.Status;
            string expected = "001006";
            //Act
            string actual = DownloadDataService.CreateMessage(dataQueryType);
            //Assert
            Assert.Equal(expected, actual);

            expected = "003006";
            actual = DownloadDataService.DownloadStatus(dataQueryType);
            Assert.Equal(expected, result.Substring(0, 6));
        }
        [Fact]
        public void Test2()
        {
        }
    }
}