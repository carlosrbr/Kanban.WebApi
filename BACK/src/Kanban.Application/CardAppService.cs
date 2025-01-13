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
    using Kanban.Domain.Validators;

    public class CardAppService : ICardAppService
    {
        private readonly ICardService _cardService;
        private readonly IMapper _mapper;
        private readonly IValidator<CardViewModel> _cardValidator;

        public CardAppService(ICardService cardService, IMapper mapper, IValidator<CardViewModel> cardValidator)
        {
            _cardService = cardService;
            _mapper = mapper;
            _cardValidator = cardValidator;
        }

        public Result<CardViewModel> Add(CardViewModel cardViewModel)
        {

            cardViewModel.Id = Guid.NewGuid();
            var validationResult = _cardValidator.Validate(cardViewModel);

            if (!validationResult.IsValid)
            {
                return new Result<CardViewModel>
                {
                    Success = false,
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                    Model = cardViewModel
                };
            }

            try
            {
                var card = _mapper.Map<Card>(cardViewModel);

                card.SetId(Guid.NewGuid());

                _cardService.Add(card);
                return new Result<CardViewModel> { Success = true, Model = _mapper.Map<CardViewModel>(card) };
            }
            catch (Exception ex)
            {
                return new Result<CardViewModel>
                {
                    Success = false,
                    Errors = new List<string> { ex.Message },
                    Model = cardViewModel
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

          

            var validationResult = _cardValidator.Validate(cardViewModel);

            if (!validationResult.IsValid)
            {
                return new Result<CardViewModel>
                {
                    Success = false,
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                    Model = cardViewModel
                };
            }

            try
            {
                var card = _mapper.Map<Card>(cardViewModel);
                var existingCard = _cardService.FindById(cardViewModel.Id);

                existingCard.SetLista(card.Lista);
                existingCard.SetTitulo(card.Titulo);
                existingCard.SetConteudo(card.Conteudo);

                _cardService.Update(existingCard);

                return new Result<CardViewModel> { Success = true, Model = _mapper.Map<CardViewModel>(card) };
            }
            catch (Exception ex)
            {
                return new Result<CardViewModel>
                {
                    Success = false,
                    Errors = [ex.Message],
                    Model = cardViewModel
                };
            } 
        }
    }
}
