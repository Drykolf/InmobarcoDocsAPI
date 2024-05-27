using InmobarcoDocsAPI.Config;
using InmobarcoDocsAPI.Core;
using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;

namespace InmobarcoDocsAPI.Endpoints;

public static class TestingEndpoints {
    private static Settings? _settings;
    public static RouteGroupBuilder MapTestingEndpoints(this WebApplication app, Settings settings) {
        _settings = settings;
        var group = app.MapGroup("/testing").WithParameterValidation();
        group.MapGet("/", () => "Testing");
        group.MapGet("/hello", () => "Hello World!");

        group.MapGet("/templates", async () => {
            try {
                Dictionary<string, string> templates = new();
                var result = await GraphHelper.GetTemplatesAsync();
                foreach (var item in result.Value) {
                    if (item.Name == null || item.Id == null) continue;
                    templates.Add(item.Name, item.Id);
                }
                return Results.Ok(templates);
            } catch (Exception ex) {
                return Results.BadRequest($"Error getting templates: {ex.Message}");
            }
        });
        return group;
    }
}
