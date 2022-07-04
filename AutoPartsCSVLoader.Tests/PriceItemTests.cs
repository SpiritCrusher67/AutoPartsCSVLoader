using AutoPartsCSVLoader.Data;
using Xunit;

namespace AutoPartsCSVLoader.Tests
{
    public class PriceItemTests
    {
        [Fact]
        public void SearchVendorPropertyDataConversion()
        {
            //Arrange
            var data = "Bosch";
            var item = new PriceItem();

            //Act
            item.SearchVendor = data;

            //Assert
            Assert.NotNull(item.SearchVendor);
            Assert.Equal("BOSCH", item.SearchVendor);
        }

        [Fact]
        public void SearchNumberPropertyDataConversion_NumbersAndSpaces()
        {
            //Arrange
            var data = "1 987 947 636";
            var item = new PriceItem();

            //Act
            item.SearchNumber = data;

            //Assert
            Assert.NotNull(item.SearchNumber);
            Assert.Equal("1987947636", item.SearchNumber);
        }

        [Fact]
        public void SearchNumberPropertyDataConversion_NonAlphaAndNumericSymbols()
        {
            //Arrange
            var data = "SA-17%12#R";
            var item = new PriceItem();

            //Act
            item.SearchNumber = data;

            //Assert
            Assert.NotNull(item.SearchNumber);
            Assert.Equal("SA1712R", item.SearchNumber);
        }

        [Fact]
        public void DescriptionPropertyDataConversion_Value512Length()
        {
            //Arrange
            var data = new string('L', 512);
            var item = new PriceItem();

            //Act
            item.Description = data;

            //Assert
            Assert.NotNull(item.Description);
            Assert.Equal(512, item.Description.Length);
        }

        [Fact]
        public void DescriptionPropertyDataConversion_ValueOver512Length()
        {
            //Arrange
            var data = new string('L', 600);
            var item = new PriceItem();

            //Act
            item.Description = data;

            //Assert
            Assert.NotNull(item.Description);
            Assert.Equal(512, item.Description.Length);
        }
    }
}