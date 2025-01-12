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
        private readonly Mock<IValidator<Card>> _mockCardValidator;
        private readonly Mock<ICardService> _cardService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CardAppService _cardAppService;

        public CardAppServiceTests()
        {
            _mockCardRepository = new Mock<ICardRepository>();
            _mockCardValidator = new Mock<IValidator<Card>>();
            _mockMapper = new Mock<IMapper>();

            // Assuming the CardService implementation is injected here
            _cardService = new Mock<ICardService>();
            _cardAppService = new CardAppService(_cardService.Object, _mockMapper.Object, _mockCardValidator.Object);
        }

        [Fact]
        public void Add_ValidCardViewModel_ReturnsSuccess()
        {
            // Arrange
            var cardViewModel = new CardViewModel { Titulo = "Test Card", Conteudo = "Conteudo 1", Lista = "TODO" };
            var card = new Card { Id = Guid.NewGuid(), Titulo = "Test Card", Conteudo = "Conteudo 1", Lista = "TODO" };

            _mockMapper.Setup(m => m.Map<Card>(cardViewModel)).Returns(card);
            _mockMapper.Setup(m => m.Map<CardViewModel>(card)).Returns(cardViewModel);
            _mockCardValidator.Setup(v => v.Validate(card)).Returns(new ValidationResult());

            //return new Result<CardViewModel> { Success = true, Model = _mapper.Map<CardViewModel>(card) };

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
            var card = new Card { Titulo = "" };
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Titulo", "Titulo é obrigatório.")
            };

            _mockMapper.Setup(m => m.Map<Card>(cardViewModel)).Returns(card);
            _mockCardValidator.Setup(v => v.Validate(card)).Returns(new ValidationResult(validationFailures));

            // Act
            var result = _cardAppService.Add(cardViewModel);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Titulo é obrigatório.", result.Errors);
            _mockMapper.Verify(m => m.Map<CardViewModel>(It.IsAny<Card>()), Times.Once);
        }

        [Fact]
        public void Delete_ExistingCard_ReturnsSuccess()
        {
            // Arrange
            var cardId = Guid.NewGuid();
            var card = new Card { Id = cardId, Titulo = "Test Card" };

            var mockCardService = new Mock<ICardService>();
            mockCardService.Setup(s => s.FindById(cardId)).Returns(card);

            var cardAppService = new CardAppService(mockCardService.Object, _mockMapper.Object, _mockCardValidator.Object);

            // Act
            var result = cardAppService.Delete(cardId);

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
            var card = new Card { Id = Guid.NewGuid(), Titulo = "Test Card", Conteudo = "Conteudo 1", Lista = "ToDo" };

            _mockMapper.Setup(m => m.Map<Card>(cardViewModel)).Returns(card);
            _mockMapper.Setup(m => m.Map<CardViewModel>(card)).Returns(cardViewModel);
            _mockCardValidator.Setup(v => v.Validate(card)).Returns(new ValidationResult() { Errors= new List<ValidationFailure>()});
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