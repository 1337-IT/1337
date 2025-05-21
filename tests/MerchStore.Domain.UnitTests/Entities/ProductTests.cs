using System;
using Xunit;
using MerchStore.Domain.Entities;

namespace MerchStore.Tests.Domain
{
    public class ProductTests
    {
        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidInputs_CreatesProduct()
        {
            // Arrange
            string name = "Test Product";
            string description = "Test Description";
            Uri imageUrl = new Uri("https://example.com/image.jpg");
            decimal price = 10.99m;
            int stockQuantity = 100;

            // Act
            var product = new Product(name, description, imageUrl, price, stockQuantity);

            // Assert
            Assert.Equal(name, product.Name);
            Assert.Equal(description, product.Description);
            Assert.Equal(imageUrl, product.ImageUrl);
            Assert.Equal(price, product.Price);
            Assert.Equal(stockQuantity, product.StockQuantity);
            Assert.NotEqual(Guid.Empty, product.Id);
        }

        [Fact]
        public void Constructor_WithNullImageUrl_CreatesProduct()
        {
            // Arrange
            string name = "Test Product";
            string description = "Test Description";
            Uri? imageUrl = null;
            decimal price = 10.99m;
            int stockQuantity = 100;

            // Act
            var product = new Product(name, description, imageUrl, price, stockQuantity);

            // Assert
            Assert.Equal(name, product.Name);
            Assert.Equal(description, product.Description);
            Assert.Null(product.ImageUrl);
            Assert.Equal(price, product.Price);
            Assert.Equal(stockQuantity, product.StockQuantity);
        }

        [Theory]
        [InlineData("", "Description", "Empty name")]
        [InlineData(null, "Description", "Null name")]
        [InlineData("   ", "Description", "Whitespace name")]
        public void Constructor_WithInvalidName_ThrowsArgumentException(string name, string description, string testCase)
        {
            // Arrange
            Uri imageUrl = new Uri("https://example.com/image.jpg");
            decimal price = 10.99m;
            int stockQuantity = 100;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                new Product(name, description, imageUrl, price, stockQuantity));
            Assert.Equal("name", exception.ParamName);
            Assert.Contains("cannot be empty", exception.Message);
        }

        [Fact]
        public void Constructor_WithTooLongName_ThrowsArgumentException()
        {
            // Arrange
            string name = new string('A', 101); // 101 characters
            string description = "Test Description";
            Uri imageUrl = new Uri("https://example.com/image.jpg");
            decimal price = 10.99m;
            int stockQuantity = 100;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                new Product(name, description, imageUrl, price, stockQuantity));
            Assert.Equal("name", exception.ParamName);
            Assert.Contains("cannot exceed 100 characters", exception.Message);
        }

        [Theory]
        [InlineData("", "Empty description")]
        [InlineData(null, "Null description")]
        [InlineData("   ", "Whitespace description")]
        public void Constructor_WithInvalidDescription_ThrowsArgumentException(string description, string testCase)
        {
            // Arrange
            string name = "Test Product";
            Uri imageUrl = new Uri("https://example.com/image.jpg");
            decimal price = 10.99m;
            int stockQuantity = 100;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                new Product(name, description, imageUrl, price, stockQuantity));
            Assert.Equal("description", exception.ParamName);
            Assert.Contains("cannot be empty", exception.Message);
        }

        [Fact]
        public void Constructor_WithTooLongDescription_ThrowsArgumentException()
        {
            // Arrange
            string name = "Test Product";
            string description = new string('A', 501); // 501 characters
            Uri imageUrl = new Uri("https://example.com/image.jpg");
            decimal price = 10.99m;
            int stockQuantity = 100;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                new Product(name, description, imageUrl, price, stockQuantity));
            Assert.Equal("description", exception.ParamName);
            Assert.Contains("cannot exceed 500 characters", exception.Message);
        }

        [Fact]
        public void Constructor_WithInvalidImageUrlProtocol_ThrowsArgumentException()
        {
            // Arrange
            string name = "Test Product";
            string description = "Test Description";
            Uri imageUrl = new Uri("ftp://example.com/image.jpg");
            decimal price = 10.99m;
            int stockQuantity = 100;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                new Product(name, description, imageUrl, price, stockQuantity));
            Assert.Equal("imageUrl", exception.ParamName);
            Assert.Contains("must use HTTP or HTTPS protocol", exception.Message);
        }

        [Fact]
        public void Constructor_WithInvalidImageUrlExtension_ThrowsArgumentException()
        {
            // Arrange
            string name = "Test Product";
            string description = "Test Description";
            Uri imageUrl = new Uri("https://example.com/document.pdf");
            decimal price = 10.99m;
            int stockQuantity = 100;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                new Product(name, description, imageUrl, price, stockQuantity));
            Assert.Equal("imageUrl", exception.ParamName);
            Assert.Contains("must point to a valid image file", exception.Message);
        }

        [Fact]
        public void Constructor_WithNegativePrice_ThrowsArgumentException()
        {
            // Arrange
            string name = "Test Product";
            string description = "Test Description";
            Uri imageUrl = new Uri("https://example.com/image.jpg");
            decimal price = -10.99m;
            int stockQuantity = 100;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                new Product(name, description, imageUrl, price, stockQuantity));
            Assert.Equal("price", exception.ParamName);
            Assert.Contains("cannot be negative", exception.Message);
        }

        [Fact]
        public void Constructor_WithNegativeStockQuantity_ThrowsArgumentException()
        {
            // Arrange
            string name = "Test Product";
            string description = "Test Description";
            Uri imageUrl = new Uri("https://example.com/image.jpg");
            decimal price = 10.99m;
            int stockQuantity = -10;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                new Product(name, description, imageUrl, price, stockQuantity));
            Assert.Equal("stockQuantity", exception.ParamName);
            Assert.Contains("cannot be negative", exception.Message);
        }

        #endregion

        #region UpdateDetails Tests

        [Fact]
        public void UpdateDetails_WithValidInputs_UpdatesProduct()
        {
            // Arrange
            var product = CreateValidProduct();
            string newName = "Updated Product";
            string newDescription = "Updated Description";
            Uri newImageUrl = new Uri("https://example.com/updated-image.png");

            // Act
            product.UpdateDetails(newName, newDescription, newImageUrl);

            // Assert
            Assert.Equal(newName, product.Name);
            Assert.Equal(newDescription, product.Description);
            Assert.Equal(newImageUrl, product.ImageUrl);
        }

        [Theory]
        [InlineData("", "Description", "Empty name")]
        [InlineData(null, "Description", "Null name")]
        [InlineData("   ", "Description", "Whitespace name")]
        public void UpdateDetails_WithInvalidName_ThrowsArgumentException(string name, string description, string testCase)
        {
            // Arrange
            var product = CreateValidProduct();
            Uri imageUrl = new Uri("https://example.com/image.jpg");

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                product.UpdateDetails(name, description, imageUrl));
            Assert.Equal("name", exception.ParamName);
            Assert.Contains("cannot be empty", exception.Message);
        }

        [Fact]
        public void UpdateDetails_WithTooLongName_ThrowsArgumentException()
        {
            // Arrange
            var product = CreateValidProduct();
            string name = new string('A', 101); // 101 characters
            string description = "Updated Description";
            Uri imageUrl = new Uri("https://example.com/image.jpg");

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                product.UpdateDetails(name, description, imageUrl));
            Assert.Equal("name", exception.ParamName);
            Assert.Contains("cannot exceed 100 characters", exception.Message);
        }

        [Theory]
        [InlineData("", "Empty description")]
        [InlineData(null, "Null description")]
        [InlineData("   ", "Whitespace description")]
        public void UpdateDetails_WithInvalidDescription_ThrowsArgumentException(string description, string testCase)
        {
            // Arrange
            var product = CreateValidProduct();
            string name = "Updated Product";
            Uri imageUrl = new Uri("https://example.com/image.jpg");

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                product.UpdateDetails(name, description, imageUrl));
            Assert.Equal("description", exception.ParamName);
            Assert.Contains("cannot be empty", exception.Message);
        }

        [Fact]
        public void UpdateDetails_WithTooLongDescription_ThrowsArgumentException()
        {
            // Arrange
            var product = CreateValidProduct();
            string name = "Updated Product";
            string description = new string('A', 501); // 501 characters
            Uri imageUrl = new Uri("https://example.com/image.jpg");

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                product.UpdateDetails(name, description, imageUrl));
            Assert.Equal("description", exception.ParamName);
            Assert.Contains("cannot exceed 500 characters", exception.Message);
        }

        [Fact]
        public void UpdateDetails_WithInvalidImageUrlProtocol_ThrowsArgumentException()
        {
            // Arrange
            var product = CreateValidProduct();
            string name = "Updated Product";
            string description = "Updated Description";
            Uri imageUrl = new Uri("ftp://example.com/image.jpg");

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                product.UpdateDetails(name, description, imageUrl));
            Assert.Equal("imageUrl", exception.ParamName);
            Assert.Contains("must use HTTP or HTTPS protocol", exception.Message);
        }

        [Fact]
        public void UpdateDetails_WithInvalidImageUrlExtension_ThrowsArgumentException()
        {
            // Arrange
            var product = CreateValidProduct();
            string name = "Updated Product";
            string description = "Updated Description";
            Uri imageUrl = new Uri("https://example.com/document.pdf");

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                product.UpdateDetails(name, description, imageUrl));
            Assert.Equal("imageUrl", exception.ParamName);
            Assert.Contains("must point to a valid image file", exception.Message);
        }

        #endregion

        #region UpdatePrice Tests

        [Fact]
        public void UpdatePrice_WithValidPrice_UpdatesPrice()
        {
            // Arrange
            var product = CreateValidProduct();
            decimal newPrice = 15.99m;

            // Act
            product.UpdatePrice(newPrice);

            // Assert
            Assert.Equal(newPrice, product.Price);
        }

        [Fact]
        public void UpdatePrice_WithZeroPrice_UpdatesPrice()
        {
            // Arrange
            var product = CreateValidProduct();
            decimal newPrice = 0m;

            // Act
            product.UpdatePrice(newPrice);

            // Assert
            Assert.Equal(newPrice, product.Price);
        }

        [Fact]
        public void UpdatePrice_WithNegativePrice_ThrowsArgumentException()
        {
            // Arrange
            var product = CreateValidProduct();
            decimal newPrice = -5.99m;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => product.UpdatePrice(newPrice));
            Assert.Equal("newPrice", exception.ParamName);
            Assert.Contains("cannot be negative", exception.Message);
        }

        #endregion

        #region UpdateStock Tests

        [Fact]
        public void UpdateStock_WithValidQuantity_UpdatesStockQuantity()
        {
            // Arrange
            var product = CreateValidProduct();
            int newQuantity = 50;

            // Act
            product.UpdateStock(newQuantity);

            // Assert
            Assert.Equal(newQuantity, product.StockQuantity);
        }

        [Fact]
        public void UpdateStock_WithZeroQuantity_UpdatesStockQuantity()
        {
            // Arrange
            var product = CreateValidProduct();
            int newQuantity = 0;

            // Act
            product.UpdateStock(newQuantity);

            // Assert
            Assert.Equal(newQuantity, product.StockQuantity);
        }

        [Fact]
        public void UpdateStock_WithNegativeQuantity_ThrowsArgumentException()
        {
            // Arrange
            var product = CreateValidProduct();
            int newQuantity = -10;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => product.UpdateStock(newQuantity));
            Assert.Equal("quantity", exception.ParamName);
            Assert.Contains("cannot be negative", exception.Message);
        }

        #endregion

        #region DecrementStock Tests

        [Fact]
        public void DecrementStock_WithAvailableStock_DecrementsAndReturnsTrue()
        {
            // Arrange
            var product = CreateValidProduct(stockQuantity: 10);
            int quantityToDecrement = 5;
            int expectedRemainingStock = 5;

            // Act
            bool result = product.DecrementStock(quantityToDecrement);

            // Assert
            Assert.True(result);
            Assert.Equal(expectedRemainingStock, product.StockQuantity);
        }

        [Fact]
        public void DecrementStock_WithDefaultQuantity_DecrementsOneAndReturnsTrue()
        {
            // Arrange
            var product = CreateValidProduct(stockQuantity: 10);
            int expectedRemainingStock = 9;

            // Act
            bool result = product.DecrementStock(); // Default quantity = 1

            // Assert
            Assert.True(result);
            Assert.Equal(expectedRemainingStock, product.StockQuantity);
        }

        [Fact]
        public void DecrementStock_WithExactAvailableStock_SetToZeroAndReturnsTrue()
        {
            // Arrange
            var product = CreateValidProduct(stockQuantity: 10);
            int quantityToDecrement = 10;
            int expectedRemainingStock = 0;

            // Act
            bool result = product.DecrementStock(quantityToDecrement);

            // Assert
            Assert.True(result);
            Assert.Equal(expectedRemainingStock, product.StockQuantity);
        }

        [Fact]
        public void DecrementStock_WithInsufficientStock_ReturnsFalseAndUnchangedStock()
        {
            // Arrange
            var product = CreateValidProduct(stockQuantity: 5);
            int quantityToDecrement = 10;
            int expectedRemainingStock = 5; // Unchanged

            // Act
            bool result = product.DecrementStock(quantityToDecrement);

            // Assert
            Assert.False(result);
            Assert.Equal(expectedRemainingStock, product.StockQuantity);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void DecrementStock_WithInvalidQuantity_ThrowsArgumentException(int quantity)
        {
            // Arrange
            var product = CreateValidProduct();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => product.DecrementStock(quantity));
            Assert.Equal("quantity", exception.ParamName);
            Assert.Contains("must be positive", exception.Message);
        }

        #endregion

        #region IncrementStock Tests

        [Fact]
        public void IncrementStock_WithValidQuantity_IncreasesStockQuantity()
        {
            // Arrange
            var product = CreateValidProduct(stockQuantity: 10);
            int quantityToIncrement = 5;
            int expectedStockQuantity = 15;

            // Act
            product.IncrementStock(quantityToIncrement);

            // Assert
            Assert.Equal(expectedStockQuantity, product.StockQuantity);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void IncrementStock_WithInvalidQuantity_ThrowsArgumentException(int quantity)
        {
            // Arrange
            var product = CreateValidProduct();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => product.IncrementStock(quantity));
            Assert.Equal("quantity", exception.ParamName);
            Assert.Contains("must be positive", exception.Message);
        }

        #endregion

        #region Helper Methods

        private Product CreateValidProduct(
            string name = "Test Product", 
            string description = "Test Description", 
            decimal price = 9.99m, 
            int stockQuantity = 100)
        {
            return new Product(
                name,
                description,
                new Uri("https://example.com/image.jpg"),
                price,
                stockQuantity
            );
        }

        #endregion
    }
}