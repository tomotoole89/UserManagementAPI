using UserManagementAPI.Models;
using UserManagementAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "User Management API is running!");

app.MapGet("/users", (IUserRepository repo) =>
{
    return repo.GetAll();
});

app.MapGet("/users/{id}", (int id, IUserRepository repo) =>
{
    var user = repo.GetById(id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

app.MapPost("/users", (User user, IUserRepository repo) =>
{
    var created = repo.Add(user);
    return Results.Created($"/users/{created.Id}", created);
});

app.MapPut("/users/{id}", (int id, User user, IUserRepository repo) =>
{
    var updated = repo.Update(id, user);
    return updated is not null ? Results.Ok(updated) : Results.NotFound();
});

app.MapDelete("/users/{id}", (int id, IUserRepository repo) =>
{
    return repo.Delete(id) ? Results.NoContent() : Results.NotFound();
});

app.Run();


/*
 * copliot used to generate the code above. with prompts also debugged an issue using swagger to test the api. as couldn't use postman. and was getting a 404 error when naigating to /swagger. also it created the repository and model files.
 * 