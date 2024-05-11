using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var todoContext = new TodoContext();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/todos", () =>
{
    return todoContext.Todos;
})
.WithName("GetTodos");

app.MapPost("/todo", ([FromBody]TodoItem todoItem) =>
{
    todoContext.Todos.Add(todoItem);
    todoContext.SaveChanges();
});

app.Run();

public class TodoContext : DbContext
{
    public DbSet<TodoItem> Todos { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION"));
}

public class TodoItem
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool Finished { get; set; }
}