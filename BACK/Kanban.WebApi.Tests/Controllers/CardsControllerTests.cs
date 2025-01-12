namespace webapi_kanban.Tests.Controllers
{
    using Kanban.Application;
    using Kanban.Domain.Entities;
    using Kanban.Domain.Interfaces.Service;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using webapi_kanban.Controllers;

    public class CardsControllerTests
    {
        private readonly Mock<ICardService> _mockCardService;
        private readonly CardsController _controller;

        public CardsControllerTests()
        {
            _mockCardService = new Mock<ICardService>();
            _controller = new CardsController(_mockCardService.Object);
        }

        [Fact]
        public void GetCards_ShouldReturnOkResult_WithListOfCards()
        {
            // Arrange
            var cards = new List<Card>
            {
                new Card { Id = Guid.NewGuid(), Titulo = "Test1", Conteudo = "Content1", Lista = "ToDo" },
                new Card { Id = Guid.NewGuid(), Titulo = "Test2", Conteudo = "Content2", Lista = "Doing" }
            };
            _mockCardService.Setup(s => s.GetAll()).Returns(cards);

            // Act
            var result = _controller.GetCards();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(cards, okResult.Value);
        }

        [Fact]
        public void CreateCard_ShouldReturnCreatedAtAction_WhenCardIsValid()
        {
            // Arrange
            var newCard = new Card { Id = Guid.NewGuid(), Titulo = "Test", Conteudo = "Content", Lista = "Done" };
            var resultModel = new Result<Card> { Success = true, Model = newCard };
            _mockCardService.Setup(s => s.Add(newCard)).Returns(resultModel);

            // Act
            var result = _controller.CreateCard(newCard);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(newCard, createdResult.Value);
        }

        [Fact]
        public void CreateCard_ShouldReturnBadRequest_WhenCardIsInvalid()
        {
            // Arrange
            var newCard = new Card { Id = Guid.NewGuid(), Titulo = "", Conteudo = "Content", Lista = "ToDo" };
            var resultModel = new Result<Card> { Success = false, Errors = new List<string> { "Title is required." } };
            _mockCardService.Setup(s => s.Add(newCard)).Returns(resultModel);

            // Act
            var result = _controller.CreateCard(newCard);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal(resultModel.Errors, badRequestResult.Value);
        }

        [Fact]
        public void UpdateCard_ShouldReturnOk_WhenCardIsUpdated()
        {
            // Arrange
            var cardId = Guid.NewGuid();
            var existingCard = new Card { Id = cardId, Titulo = "Old", Conteudo = "OldContent", Lista = "ToDo" };
            var updatedCard = new Card { Id = cardId, Titulo = "Updated", Conteudo = "UpdatedContent", Lista = "Done" };

            _mockCardService.Setup(s => s.FindById(cardId)).Returns(existingCard);
            _mockCardService.Setup(s => s.Update(existingCard)).Returns(new Result<Card> { Success = true , Model = updatedCard });

            // Act
            var result = _controller.UpdateCard(cardId, updatedCard);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(updatedCard, okResult.Value);
        }

        [Fact]
        public void UpdateCard_ShouldReturnNotFound_WhenCardDoesNotExist()
        {
            // Arrange
            var cardId = Guid.NewGuid();
            var updatedCard = new Card { Id = cardId, Titulo = "Updated", Conteudo = "UpdatedContent", Lista = "Done" };

            _mockCardService.Setup(s => s.FindById(cardId)).Returns((Card)null);

            // Act
            var result = _controller.UpdateCard(cardId, updatedCard);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteCard_ShouldReturnOkResult_WithRemainingCards()
        {
            // Arrange
            var cardId = Guid.NewGuid();
            var remainingCards = new List<Card>
            {
                new Card { Id = Guid.NewGuid(), Titulo = "Test1", Conteudo = "Content1", Lista = "Doing" }
            };
            _mockCardService.Setup(s => s.GetAll()).Returns(remainingCards);

            // Act
            var result = _controller.DeleteCard(cardId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(remainingCards, okResult.Value);
        }
    }
}
