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

        group.MapGet("/drives", async () => {
            try {
                var drives = await GraphHelper.MakeGraphCallAsync();
                return Results.Ok(drives);
            } catch (Exception ex) {
                return Results.BadRequest($"Error getting drives: {ex.Message}");
            }
        });

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

        group.MapGet("/download", async () => {
            try {
                var tenant = await GraphHelper.GetFile("01QJDHBV2NCOE6B3AXNFCLGSGS4YC34NTX");
                return Results.Ok();
            } catch (Exception ex) {
                return Results.BadRequest($"Error getting tenant: {ex.Message}");
            }
        });

        group.MapPost("/folder", async (string folderName) => {
            try {
                var folder = await GraphHelper.CreateFolder(folderName);
                return Results.Ok(folder);
            } catch (Exception ex) {
                return Results.BadRequest($"Error creating folder: {ex.Message}");
            }
        });
        return group;

    }
}
