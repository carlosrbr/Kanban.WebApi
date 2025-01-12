namespace Kanban.Application.ViewModels
{
    using System;

    public class CardViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public string Lista { get; set; }
    }
}
