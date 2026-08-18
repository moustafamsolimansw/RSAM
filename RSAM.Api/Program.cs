using RSAM.Api;
using RSAM.Application;
using RSAM.Infrastructure;

Console.WriteLine("BEFORE create the web application");
var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("AFTER create the web application");

// Add services to the container.
Console.WriteLine("BEFORE DI registeration");

builder.Services.AddApi().AddApplication().AddInfrastructure(builder.Configuration);
Console.WriteLine("AFTER DI registeration");

Console.WriteLine("BEFORE application build");

var app = builder.Build();
Console.WriteLine("AFTER application build");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RSAM API v1");
});

app.Run();
