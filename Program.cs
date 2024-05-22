using InmobarcoDocsAPI.Config;
using InmobarcoDocsAPI.Core;
using InmobarcoDocsAPI.Endpoints;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.GetValue<string>("ConnectionStrings:AzureStorage");
var settings = Settings.LoadSettings();
InitializeGraph(settings);
var app = builder.Build();
app.Logger.LogInformation("Hello World!");


//if (app.Environment.IsDevelopment()) {}

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");
app.MapContractsEndpoints(settings).WithOpenApi();
app.MapTestingEndpoints(settings).WithOpenApi();
app.Run();


void InitializeGraph(Settings settings) {
    GraphHelper.InitializeGraphForAppOnlyAuth(settings);
}