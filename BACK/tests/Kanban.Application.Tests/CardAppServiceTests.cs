namespace Kanban.Domain.Tests.Validators
{
    using AutoMapper;
    using FluentValidation;
    using FluentValidation.Results;
    using Kanban.Application;
    using Kanban.Application.ViewModels;
    using Kanban.Domain.Entities;
    using Kanban.Domain.Interfaces.Repository;
    using Kanban.Domain.Interfaces.Service;
    using Moq;
    using Xunit;

    public class CardAppServiceTests
    {
        private readonly Mock<ICardRepository> _mockCardRepository;
        private readonly Mock<IValidator<CardViewModel>> _mockCardValidator;
        private readonly Mock<ICardService> _cardService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CardAppService _cardAppService;

        public CardAppServiceTests()
        {
            _mockCardRepository = new Mock<ICardRepository>();
            _mockCardValidator = new Mock<IValidator<CardViewModel>>();
            _mockMapper = new Mock<IMapper>();

            // Assuming the CardService implementation is injected here
            _cardService = new Mock<ICardService>();
            _cardAppService = new CardAppService(_cardService.Object, _mockMapper.Object, _mockCardValidator.Object);
        }

        [Fact]
        public void Add_ValidCardViewModel_ReturnsSuccess()
        {
            // Arrange
            var cardViewModel = new CardViewModel { Titulo = "Test Card", Conteudo = "Conteudo 1", Lista = "ToDo" };
            var card = new Card(Guid.NewGuid(), "Test Card", "Conteudo 1", "ToDo");

            _mockMapper.Setup(m => m.Map<Card>(cardViewModel)).Returns(card);
            _mockMapper.Setup(m => m.Map<CardViewModel>(card)).Returns(cardViewModel);
            _mockCardValidator.Setup(v => v.Validate(cardViewModel)).Returns(new ValidationResult());

            // Act
            var result = _cardAppService.Add(cardViewModel);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Model);
            _mockMapper.Verify(m => m.Map<CardViewModel>(It.IsAny<Card>()), Times.Once);
        }

        [Fact]
        public void Add_InvalidCardViewModel_ReturnsFailure()
        {
            // Arrange
            var cardViewModel = new CardViewModel { Titulo = "" };
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Titulo", "Titulo é obrigatório.")
            };

            _mockCardValidator.Setup(v => v.Validate(cardViewModel)).Returns(new ValidationResult(validationFailures));

            // Act
            var result = _cardAppService.Add(cardViewModel);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Titulo é obrigatório.", result.Errors);
        }

        [Fact]
        public void Delete_ExistingCard_ReturnsSuccess()
        {
            // Arrange
            var card = new Card("Test Card", "Conteundo", "ToDo");

            var mockCardService = new Mock<ICardService>();
            mockCardService.Setup(s => s.FindById(card.Id)).Returns(card);

            var cardAppService = new CardAppService(mockCardService.Object, _mockMapper.Object, _mockCardValidator.Object);

            // Act
            var result = cardAppService.Delete(card.Id);

            // Assert
            Assert.True(result.Success);
            _mockMapper.Verify(m => m.Map<CardViewModel>(It.IsAny<Card>()), Times.Once);
        }

        [Fact]
        public void Delete_NonExistingCard_ReturnsFailure()
        {
            // Arrange
            var cardId = Guid.NewGuid();

            var mockCardService = new Mock<ICardService>();
            mockCardService.Setup(s => s.FindById(cardId)).Returns((Card)null);

            var cardAppService = new CardAppService(mockCardService.Object, _mockMapper.Object, _mockCardValidator.Object);

            // Act
            var result = cardAppService.Delete(cardId);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("card não encontrado", result.Errors);
        }
        [Fact]
        public void Update_ValidCard_ReturnsSuccess()
        {
            // Arrange
            var cardViewModel = new CardViewModel { Titulo = "Test Card", Conteudo = "Conteudo 1", Lista = "ToDo" };
            var card = new Card("Test Card", "Conteudo 1", "ToDo");

            _mockMapper.Setup(m => m.Map<Card>(cardViewModel)).Returns(card);
            _mockMapper.Setup(m => m.Map<CardViewModel>(card)).Returns(cardViewModel);
            _mockCardValidator.Setup(v => v.Validate(cardViewModel)).Returns(new ValidationResult() { Errors = new List<ValidationFailure>() });
            _cardService.Setup(v => v.FindById(cardViewModel.Id)).Returns(card);
            // Act
            var result = _cardAppService.Update(cardViewModel);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Model);
            _mockMapper.Verify(m => m.Map<CardViewModel>(It.IsAny<Card>()), Times.Once);
        }
    }
}