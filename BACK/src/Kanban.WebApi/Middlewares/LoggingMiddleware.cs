namespace webapi_kanban.Middlewares
{
   public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Method == "PUT" || context.Request.Method == "DELETE")
        {
            var id = context.Request.Path.Value?.Split('/').Last();
            Console.WriteLine($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Card {id} - {(context.Request.Method ==  "PUT" ? "Alterado" : "Removido")}");
        }

        await _next(context);
    }
}
}
