namespace Kanban.Domain.Entities
{
    public class Card
    {
        public Guid Id { get; private set; }
        public string Titulo { get; private set; }
        public string Conteudo { get; private set; }
        public string Lista { get; private set; }

        public Card(string titulo, string conteudo, string lista)
        {
            Id = Guid.NewGuid();
            SetTitulo(titulo);
            SetConteudo(conteudo);
            SetLista(lista);
        }
        public Card(Guid id, string titulo, string conteudo, string lista)
        {
            Id = id;
            SetTitulo(titulo);
            SetConteudo(conteudo);
            SetLista(lista);
        }
        public void SetId(Guid id)
        {
            Id = id;
        }
        public void SetTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo) || titulo.Length < 3)
                throw new ArgumentException("Titulo deve ter ao menos 3 caracteres.");
            Titulo = titulo;
        }

        public void SetConteudo(string conteudo)
        {
            if (string.IsNullOrWhiteSpace(conteudo) || conteudo.Length < 3)
                throw new ArgumentException("Conteudo deve ter ao menos 3 caracteres.");
            Conteudo = conteudo;
        }

        public void SetLista(string lista)
        {
            var listasValidas = new[] { "Doing", "ToDo", "Done" };
            if (!listasValidas.Contains(lista))
                throw new ArgumentException("Lista deve ser um dos itens ('Doing', 'ToDo', 'Done').");
            Lista = lista;
        }
    }
}
