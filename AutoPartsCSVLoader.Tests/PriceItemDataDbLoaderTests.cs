using AutoPartsCSVLoader.Data;
using AutoPartsCSVLoader.Services;
using AutoPartsCSVLoader.Tests.Common;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace AutoPartsCSVLoader.Tests
{
    public class PriceItemDataDbLoaderTests
    {
        [Fact]
        public async Task PriceItemDataDbLoaderLoadDataFromFileWithBadData_()
        {
            //Arrange
            var repo = new TestPriceItemRepository();
            var loggerMoq = new Mock<ILogger<PriceItemDataDbLoader>>();
            var logger = loggerMoq.Object;
            var dbLoader = new PriceItemDataDbLoader(logger, repo, TestConfigurationProvider.Configuration);
            var path = Path.Combine(Environment.CurrentDirectory, @"TestData\", "TestDataWithBadRowsFromSupplier1.csv");
            Stream stream = new FileStream(path, FileMode.Open);

            var expectedCount = TestDataProvider.TestPriceItems.Count;
            var expectedList = TestDataProvider.TestPriceItems;

            //Act
            await dbLoader.LoadDataAsync(stream, "Supplier1");

            //Assert
            Assert.Equal(4, repo.PriceItems.Count);
            Assert.Equal(2, loggerMoq.Invocations.Count);
        }
        [Fact]
        public async Task PriceItemDataDbLoaderLoadDataFrom_Supplier1()
        {
            await PriceItemDataDbLoaderLoadDataFromFile("TestDataFromSupplier1.csv", "Supplier1");
        }

        [Fact]
        public async Task PriceItemDataDbLoaderLoadDataFrom_Supplier2()
        {
            await PriceItemDataDbLoaderLoadDataFromFile("TestDataFromSupplier2.csv", "Supplier2");
        }

        private async Task PriceItemDataDbLoaderLoadDataFromFile(string fileName, string supplierName)
        {
            //Arrange
            var repo = new TestPriceItemRepository();
            var logger = new Mock<ILogger<PriceItemDataDbLoader>>().Object;
            var dbLoader = new PriceItemDataDbLoader(logger, repo, TestConfigurationProvider.Configuration);

            var path = Path.Combine(Environment.CurrentDirectory, @"TestData\", fileName);
            Stream stream = new FileStream(path, FileMode.Open);

            var expectedCount = TestDataProvider.TestPriceItems.Count;
            var expectedList = TestDataProvider.TestPriceItems;

            //Act
            await dbLoader.LoadDataAsync(stream, supplierName);

            //Assert
            Assert.Equal(expectedCount, repo.PriceItems.Count);
            for (int i = 0; i < expectedCount; i++)
            {
                ComparePriceItems(expectedList[i], repo.PriceItems[i]);
            }
        }

        private void ComparePriceItems(PriceItem expected, PriceItem actual)
        {
            Assert.Equal(expected.Number, actual.Number);
            Assert.Equal(expected.SearchNumber, actual.SearchNumber);
            Assert.Equal(expected.Vendor, actual.Vendor);
            Assert.Equal(expected.SearchVendor, actual.SearchVendor);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.Count, actual.Count);
            Assert.Equal(expected.Price, actual.Price);
        }
    }
}
