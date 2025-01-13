namespace Kanban.Domain.Validators
{
    using FluentValidation;
    using Kanban.Application.ViewModels;
    using Kanban.Domain.Entities;

    public class CardValidator : AbstractValidator<CardViewModel>
    {
        public CardValidator()
        {
            RuleFor(card => card.Id)
                .NotEmpty().WithMessage("O Id não pode ser vazio.");

            RuleFor(card => card.Titulo)
                .NotEmpty().WithMessage(" ")
                .MinimumLength(3).WithMessage("Titulo deve ter ao meno 3 caracteres.");

            RuleFor(card => card.Conteudo)
                .NotEmpty().WithMessage("Conteudo is required.")
                .MinimumLength(3).WithMessage("Conteudo deve ter ao meno 3 caracteres.");

            RuleFor(card => card.Lista)
                .Must(lista => lista == "Doing" || lista == "ToDo" || lista == "Done")
                .WithMessage("Lista deve ser um dos itens ('Doing','ToDo','Done').");
        }
    }
}
 