using FluentValidation;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;
using UserManagementAPI.Middleware;
using UserManagementAPI.Models;
using UserManagementAPI.Repositories;
using UserManagementAPI.Services;


var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



builder.Services.AddScoped<IValidator<User>, UserValidator>();



var app = builder.Build();

//middleware registration order matters
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<TokenValidationMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();


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

app.MapPost("/users", async (User user, IUserRepository repo, IValidator<User> validator) =>
{
    var result = await validator.ValidateAsync(user);

    if (!result.IsValid)
        return Results.BadRequest(result.Errors);

    var created = repo.Add(user);
    return Results.Created($"/users/{created.Id}", created);
});



app.MapPut("/users/{id}", async (int id, User user, IUserRepository repo, IValidator<User> validator) =>
{
    var result = await validator.ValidateAsync(user);

    if (!result.IsValid)
    {
        return Results.BadRequest(result.Errors);
    }

    var updated = repo.Update(id, user);

    return updated is not null
        ? Results.Ok(updated)
        : Results.NotFound(new { message = $"User with ID {id} does not exist." });
});



app.MapDelete("/users/{id}", (int id, IUserRepository repo) =>
{
    return repo.Delete(id) ? Results.NoContent() : Results.NotFound();
});

app.Run();


