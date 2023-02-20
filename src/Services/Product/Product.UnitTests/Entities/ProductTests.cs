using FluentAssertions;

namespace Product.UnitTests.Entities
{
    public class ProductTests
    {
        [Fact]
        public void Update_WithNullName_ThrowArgumentException()
        {
            // Arrange
            API.Domain.Entities.Product product = new(
                id: Guid.NewGuid(),
                name: "Soap",
                value: 1.5,
                active: true
            );

            // Act
            Action action = () => product.Update(name: null, value: 2);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("'name' cannot be null or empty. (Parameter 'name')");
        }

        [Fact]
        public void Update_WithEmptyName_ThrowArgumentException()
        {
            // Arrange
            API.Domain.Entities.Product product = new(
                id: Guid.NewGuid(),
                name: "Soap",
                value: 1.5,
                active: true
            );

            // Act
            Action action = () => product.Update(name: string.Empty, value: 2);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("'name' cannot be null or empty. (Parameter 'name')");
        }

        [Theory]
        [InlineData("a")]
        [InlineData("aa")]
        [InlineData("aaa")]
        public void Update_NameWithInvalidCharacters_ThrowArgumentException(string newName)
        {
            // Arrange
            API.Domain.Entities.Product product = new(
                id: Guid.NewGuid(),
                name: "Soap",
                value: 1.5,
                active: true
            );

            // Act
            Action action = () => product.Update(name: newName, value: 2);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("'name' must be more than 3 characters. (Parameter 'name')");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Update_WithInvalidValue_ThrowArgumentException(double value)
        {
            // Arrange
            API.Domain.Entities.Product product = new(
                id: Guid.NewGuid(),
                name: "Soap",
                value: 1.5,
                active: true
            );

            // Act
            Action action = () => product.Update(name: "Soap", value);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("'value' cannot be less or equal to zero (Parameter 'value')");
        }

        [Theory]
        [InlineData("Kit Kat", 2.0)]
        [InlineData("M&M's", 0.0001)]
        [InlineData("Snickers", 10)]
        public void Update_WithValidValues_UpdateProductDataCorrectly(string name, double value)
        {
            // Arrange
            API.Domain.Entities.Product product = new(
                id: Guid.NewGuid(),
                name: "Soap",
                value: 1.5,
                active: true
            );

            // Act
            product.Update(name, value);

            // Assert
            product.Id.Should().Be(product.Id);
            product.Name.Should().Be(name);
            product.Value.Should().Be(value);
            product.Active.Should().BeTrue();
        }
    }
}
