namespace Kanban.Domain.Tests.Validators
{

    using FluentValidation;
    using FluentValidation.Results;
    using Kanban.Application;
    using Kanban.Domain.Entities;
    using Kanban.Domain.Interfaces.Repository;
    using Moq;
    using Xunit;

    public class CardServiceTests
    {
        private readonly Mock<ICardRepository> _mockCardRepository;
        private readonly Mock<IValidator<Card>> _mockCardValidator;
        private readonly CardService _cardService;

        public CardServiceTests()
        {
            _mockCardRepository = new Mock<ICardRepository>();
            _mockCardValidator = new Mock<IValidator<Card>>();
            _cardService = new CardService(_mockCardRepository.Object, _mockCardValidator.Object);
        }

        [Fact]
        public void Add_ValidCard_ReturnsSuccess()
        {
            // Arrange
            var card = new Card { Titulo = "Test Card", Conteudo = "Conteudo 1", Lista = "TODO" };
            _mockCardValidator.Setup(v => v.Validate(card)).Returns(new ValidationResult());

            // Act
            var result = _cardService.Add(card);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Model);
            _mockCardRepository.Verify(r => r.Add(It.IsAny<Card>()), Times.Once);
        }

        [Fact]
        public void Add_InvalidCard_ReturnsFailure()
        {
            // Arrange
            var card = new Card { Titulo = "" };
            var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Titulo", "Titulo é obrigatório.")
        };
            _mockCardValidator.Setup(v => v.Validate(card)).Returns(new ValidationResult(validationFailures));

            // Act
            var result = _cardService.Add(card);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Titulo é obrigatório.", result.Errors);
            _mockCardRepository.Verify(r => r.Add(It.IsAny<Card>()), Times.Never);
        }

        [Fact]
        public void Delete_ExistingCard_ReturnsSuccess()
        {
            // Arrange
            var cardId = Guid.NewGuid();
            var card = new Card { Id = cardId, Titulo = "Test Card" };
            _mockCardRepository.Setup(r => r.FindById(cardId)).Returns(card);

            // Act
            var result = _cardService.Delete(cardId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(card, result.Model);
            _mockCardRepository.Verify(r => r.Delete(cardId), Times.Once);
        }

        [Fact]
        public void Delete_NonExistingCard_ReturnsFailure()
        {
            // Arrange
            var cardId = Guid.NewGuid();
            _mockCardRepository.Setup(r => r.FindById(cardId)).Returns((Card)null);

            // Act
            var result = _cardService.Delete(cardId);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("card não encontrado", result.Errors);
            _mockCardRepository.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public void Update_ValidCard_ReturnsSuccess()
        {
            // Arrange
            var card = new Card { Id = Guid.NewGuid(), Titulo = "Updated Card", Conteudo = "Conteudo", Lista = "ToDo" };
            _mockCardValidator.Setup(v => v.Validate(card)).Returns(new ValidationResult());

            // Act
            var result = _cardService.Update(card);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(card, result.Model);
            _mockCardRepository.Verify(r => r.Update(card), Times.Once);
        }

        [Fact]
        public void Update_InvalidCard_ReturnsFailure()
        {
            // Arrange
            var card = new Card { Id = Guid.NewGuid(), Titulo = "" };
            var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Titulo", "Titulo é obrigatório.")
        };
            _mockCardValidator.Setup(v => v.Validate(card)).Returns(new ValidationResult(validationFailures));

            // Act
            var result = _cardService.Update(card);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Titulo é obrigatório.", result.Errors);
            _mockCardRepository.Verify(r => r.Update(It.IsAny<Card>()), Times.Never);
        }

        [Fact]
        public void FindById_ExistingCard_ReturnsCard()
        {
            // Arrange
            var cardId = Guid.NewGuid();
            var card = new Card { Id = cardId, Titulo = "Test Card", Conteudo = "Test Conteudo 1", Lista = "ToDo" };
            _mockCardRepository.Setup(r => r.FindById(cardId)).Returns(card);

            // Act
            var result = _cardService.FindById(cardId);

            // Assert
            Assert.Equal(card, result);
        }

        [Fact]
        public void FindById_NonExistingCard_ReturnsNull()
        {
            // Arrange
            var cardId = Guid.NewGuid();
            _mockCardRepository.Setup(r => r.FindById(cardId)).Returns((Card)null);

            // Act
            var result = _cardService.FindById(cardId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAll_ReturnsAllCards()
        {
            // Arrange
            var cards = new List<Card>
        {
            new Card { Id = Guid.NewGuid(), Titulo = "Card 1", Conteudo  ="Conteudo 1",Lista="ToDo" },
        };

            _mockCardRepository.Setup(r => r.GetAll()).Returns(cards);

            // Act
            var result = _cardService.GetAll();

            // Assert
            Assert.Equal(cards, result);
        }
    }
}