using UserManagementAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// In Program.cs, just before app.MapControllers();
app.UseExceptionHandler("/error"); // Or a custom path for error handling

// Custom middleware: Logging and Token Validation
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<TokenValidationMiddleware>();


app.UseHttpsRedirection();

app.UseAuthorization();


// Add a simple error controller or map for testing purposes
// This is just a basic example; a real error handling controller would be more elaborate
app.Map("/error", (HttpContext context) =>
{
    var exceptionHandlerPathFeature =
        context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

    var exception = exceptionHandlerPathFeature?.Error;
    // Log the exception here
    Console.WriteLine($"Unhandled exception: {exception?.Message}");

    return Results.Problem(
        detail: "An unexpected error occurred. Please try again later.",
        statusCode: StatusCodes.Status500InternalServerError,
        title: "Internal Server Error"
    );
});

app.MapControllers();

app.Run();
