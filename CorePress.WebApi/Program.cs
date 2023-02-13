var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/", () => new {
    messege = "Hello World"
});

app.Run();