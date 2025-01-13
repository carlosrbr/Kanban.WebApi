
namespace Kanban.Domain.Entities.Tests
{
    using System;
    using Xunit;

    public class CardTests
    {
        [Fact]
        public void CreateCard_ShouldReturnCard_WhenAllParametersAreValid()
        {
            // Arrange
            string title = "Titulo";
            string content = "Conteudo1";
            string list = "ToDo";

            // Act
            var card = new Card(title, content, list);

            // Assert
            Assert.NotNull(card);
            Assert.Equal(title, card.Titulo);
            Assert.Equal(content, card.Conteudo);
            Assert.Equal(list, card.Lista);
        }

        [Fact]
        public void SetTitle_ShouldThrowArgumentException_WhenTitleIsInvalid()
        {
            // Arrange
            var card = new Card("Title1", "Conteudo", "ToDo");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => card.SetTitulo("Ti"));
            Assert.Equal("Titulo deve ter ao menos 3 caracteres.", ex.Message);
        }

        [Fact]
        public void SetContent_ShouldThrowArgumentException_WhenContentIsInvalid()
        {
            // Arrange
            var card = new Card("Titulo", "Content1", "ToDo");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => card.SetConteudo("Co"));
            Assert.Equal("Conteudo deve ter ao menos 3 caracteres.", ex.Message);
        }

        [Fact]
        public void SetList_ShouldThrowArgumentException_WhenListIsInvalid()
        {
            // Arrange
            var card = new Card("Titulo", "Conteudo", "ToDo");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => card.SetLista("Invalid"));
            Assert.Equal("Lista deve ser um dos itens ('Doing', 'ToDo', 'Done').", ex.Message);
        }

        [Fact]
        public void SetList_ShouldSetListCorrectly_WhenListIsValid()
        {
            // Arrange
            var card = new Card("Titulo", "Conteudo", "ToDo");

            // Act
            card.SetLista("Done");

            // Assert
            Assert.Equal("Done", card.Lista);
        }

        [Fact]
        public void SetId_ShouldUpdateId_WhenIdIsValid()
        {
            // Arrange
            var card = new Card("Titulo", "Conteudo", "ToDo");
            var newId = Guid.NewGuid();

            // Act                
            card.SetId(newId);

            // Assert
            Assert.Equal(newId, card.Id);
        }
    }
}
