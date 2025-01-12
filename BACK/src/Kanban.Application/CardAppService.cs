namespace Kanban.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using FluentValidation;
    using Kanban.Application.Interfaces;
    using Kanban.Application.ViewModels;
    using Kanban.Domain.Entities;
    using Kanban.Domain.Interfaces.Service;

    public class CardAppService : ICardAppService
    {
        private readonly ICardService _cardService;
        private readonly IMapper _mapper;
        private readonly IValidator<Card> _cardValidator;

        public CardAppService(ICardService cardService, IMapper mapper, IValidator<Card> cardValidator)
        {
            _cardService = cardService;
            _mapper = mapper;
            _cardValidator = cardValidator;
        }

        public Result<CardViewModel> Add(CardViewModel cardViewModel)
        {
            var card = _mapper.Map<Card>(cardViewModel);

            card.Id = Guid.NewGuid();
            var validationResult = _cardValidator.Validate(card);

            if (!validationResult.IsValid)
            {
                return new Result<CardViewModel>
                {
                    Success = false,
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                    Model = _mapper.Map<CardViewModel>(card)
                };
            }

            try
            {
                _cardService.Add(card);
                return new Result<CardViewModel> { Success = true, Model = _mapper.Map<CardViewModel>(card) };
            }
            catch (Exception ex)
            {
                return new Result<CardViewModel>
                {
                    Success = false,
                    Errors = new List<string> { ex.Message },
                    Model = _mapper.Map<CardViewModel>(card)
                };
            }
        }

        public Result<CardViewModel> Delete(Guid card)
        {
            var existingCard = _cardService.FindById(card);

            if (existingCard == null)
            {
                return new Result<CardViewModel>
                {
                    Success = false,
                    Errors = new List<string> { "card não encontrado" }
                };

            }

            _cardService.Delete(card);

            return new Result<CardViewModel>
            {
                Success = true,
                Model = _mapper.Map<CardViewModel>(existingCard!)
            };
        }

        public CardViewModel FindById(Guid id)
        {
            return _mapper.Map<CardViewModel>(_cardService.FindById(id));
        }

        public IEnumerable<CardViewModel> GetAll()
        {
            return _mapper.Map<IEnumerable<CardViewModel>>(_cardService.GetAll());
        }

        public Result<CardViewModel> Update(CardViewModel cardViewModel)
        {

            var card = _mapper.Map<Card>(cardViewModel);

            var validationResult = _cardValidator.Validate(card);

            if (!validationResult.IsValid)
            {
                return new Result<CardViewModel>
                {
                    Success = false,
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                    Model = _mapper.Map<CardViewModel>(card)
                };
            }

            try
            {
                var existingCard = _cardService.FindById(cardViewModel.Id);

                existingCard.Lista = cardViewModel.Lista;
                existingCard.Titulo = card.Titulo;
                existingCard.Conteudo = card.Conteudo;

                _cardService.Update(existingCard);

                return new Result<CardViewModel> { Success = true, Model = _mapper.Map<CardViewModel>(card) };
            }
            catch (Exception ex)
            {
                return new Result<CardViewModel>
                {
                    Success = false,
                    Errors = [ex.Message],
                    Model = _mapper.Map<CardViewModel>(card)
                };
            } 
        }
    }
}
