namespace Kanban.Domain.Interfaces.Service
{
    using System.Collections.Generic;

    public class Result<T>
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public T Model { get; set; }
    }
}
