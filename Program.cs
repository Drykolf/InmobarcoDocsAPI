using InmobarcoDocsAPI.Endpoints;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.GetValue<string>("ConnectionStrings:AzureStorage");
var app = builder.Build();
app.Logger.LogInformation("Hello World!");

//if (app.Environment.IsDevelopment()) {}

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");
app.MapContractsEndpoints().WithOpenApi();
app.Run();
