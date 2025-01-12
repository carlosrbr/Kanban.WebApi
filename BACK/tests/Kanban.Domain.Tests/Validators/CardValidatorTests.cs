namespace Kanban.Domain.Tests.Validators
{
    using FluentValidation.Results;
    using Kanban.Domain.Entities;
    using Kanban.Domain.Validators;
    using Xunit;

    public class CardValidatorTests
    {
        private readonly CardValidator _validator;

        public CardValidatorTests()
        {
            _validator = new CardValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Empty()
        {
            // Arrange
            var card = new Card { Id = Guid.Empty, Titulo = "Valid Title", Conteudo = "Valid Content", Lista = "Doing" };

            // Act
            ValidationResult result = _validator.Validate(card);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Id" && e.ErrorMessage == "O Id não pode ser vazio.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Id_Is_Valid()
        {
            // Arrange
            var card = new Card { Id = Guid.NewGuid(), Titulo = "Valid Title", Conteudo = "Valid Content", Lista = "Doing" };

            // Act
            ValidationResult result = _validator.Validate(card);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Have_Error_When_Titulo_Is_Empty()
        {
            // Arrange
            var card = new Card { Id = Guid.NewGuid(), Titulo = string.Empty, Conteudo = "Valid Content", Lista = "ToDo" };

            // Act
            ValidationResult result = _validator.Validate(card);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Titulo" && e.ErrorMessage == " ");
        }

        [Fact]
        public void Should_Have_Error_When_Titulo_Is_Too_Short()
        {
            // Arrange
            var card = new Card { Id = Guid.NewGuid(), Titulo = "AA", Conteudo = "Valid Content", Lista = "ToDo" };

            // Act
            ValidationResult result = _validator.Validate(card);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Titulo" && e.ErrorMessage == "Titulo deve ter ao meno 3 caracteres.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Titulo_Is_Valid()
        {
            // Arrange
            var card = new Card { Id = Guid.NewGuid(), Titulo = "Titulo1", Conteudo = "Valid Content", Lista = "ToDo" };

            // Act
            ValidationResult result = _validator.Validate(card);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Have_Error_When_Conteudo_Is_Empty()
        {
            // Arrange
            var card = new Card { Id = Guid.NewGuid(), Titulo = "Titulo", Conteudo = string.Empty, Lista = "ToDo" };

            // Act
            ValidationResult result = _validator.Validate(card);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Conteudo" && e.ErrorMessage == "Conteudo is required.");
        }

        [Fact]
        public void Should_Have_Error_When_Conteudo_Is_Too_Short()
        {
            // Arrange
            var card = new Card { Id = Guid.NewGuid(), Titulo = "Titulo", Conteudo = "CC", Lista = "ToDo" };

            // Act
            ValidationResult result = _validator.Validate(card);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Conteudo" && e.ErrorMessage == "Conteudo deve ter ao meno 3 caracteres.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Conteudo_Is_Valid()
        {
            // Arrange
            var card = new Card { Id = Guid.NewGuid(), Titulo = "Titulo", Conteudo = "Valid Content", Lista = "ToDo" };

            // Act
            ValidationResult result = _validator.Validate(card);

            // Assert
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("Doing")]
        [InlineData("ToDo")]
        [InlineData("Done")]
        public void Should_Not_Have_Error_When_Lista_Is_Valid(string lista)
        {
            // Arrange
            var card = new Card { Id = Guid.NewGuid(), Titulo = "Titulo", Conteudo = "Valid Content", Lista = lista };

            // Act
            ValidationResult result = _validator.Validate(card);

            // Assert
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("Invalid")]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Have_Error_When_Lista_Is_Invalid(string lista)
        {
            // Arrange
            var card = new Card { Id = Guid.NewGuid(), Titulo = "Titulo", Conteudo = "Valid Content", Lista = lista };

            // Act
            ValidationResult result = _validator.Validate(card);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Lista" && e.ErrorMessage == "Lista deve ser um dos itens ('Doing','ToDo','Done').");
        }
    }

}