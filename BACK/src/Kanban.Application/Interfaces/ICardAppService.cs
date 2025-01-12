namespace Kanban.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Kanban.Application.ViewModels;
    using Kanban.Domain.Interfaces.Service;

    public interface ICardAppService
    {
        Result<CardViewModel> Add(CardViewModel card);

        CardViewModel FindById(Guid id);

        Result<CardViewModel> Update(CardViewModel card);

        Result<CardViewModel> Delete(Guid card);

        IEnumerable<CardViewModel> GetAll();
    }
}
