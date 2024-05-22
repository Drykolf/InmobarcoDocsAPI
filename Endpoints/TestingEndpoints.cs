using InmobarcoDocsAPI.Config;
using InmobarcoDocsAPI.Core;

namespace InmobarcoDocsAPI.Endpoints;

public static class TestingEndpoints {
    private static Settings? _settings;
    public static RouteGroupBuilder MapTestingEndpoints(this WebApplication app, Settings settings) {
        _settings = settings;
        var group = app.MapGroup("/testing").WithParameterValidation();
        group.MapGet("/", () => "Testing");
        group.MapGet("/hello", () => "Hello World!");
        group.MapGet("/settings", () => {
            return new {
                _settings.ClientId,
                _settings.TenantId,
                _settings.ClientSecret
            };
        });

        group.MapGet("/token", async () => {
            try {
                var appOnlyToken = await GraphHelper.GetAppOnlyTokenAsync();
                return Results.Ok(appOnlyToken);
            } catch (Exception ex) {
                return Results.BadRequest($"Error getting app-only access token: {ex.Message}");
            }
        });

        group.MapGet("/users", async () => {
            try {
                var userPage = await GraphHelper.GetUsersAsync();

                if (userPage?.Value == null) {
                    return Results.Ok("No results returned.");
                }
                Dictionary<string, Dictionary<string, string>> users = new();
                // Output each users's details
                foreach (var user in userPage.Value) {
                    Dictionary<string, string> userInfo = new() {
                        { "ID", user.Id ?? "NO ID"},
                        { "Email", user.Mail ?? "NO EMAIL" }
                    };
                    users.Add(user.DisplayName ?? "NO NAME", userInfo);
                }

                // If NextPageRequest is not null, there are more users
                // available on the server
                // Access the next page like:
                // var nextPageRequest = new UsersRequestBuilder(userPage.OdataNextLink, _appClient.RequestAdapter);
                // var nextPage = await nextPageRequest.GetAsync();
                var moreAvailable = !string.IsNullOrEmpty(userPage.OdataNextLink);
                if (moreAvailable) {
                    users.Add("MoreAvailable", new Dictionary<string, string> {
                        { "NextPage", userPage.OdataNextLink ?? "NO NEXT PAGE" }
                    });
                }
                return Results.Ok(users);
            } catch (Exception ex) {
                return Results.BadRequest($"Error getting users: {ex.Message}");
            }
        });

        group.MapGet("/drives", async () => {
            try {
                var drives = await GraphHelper.MakeGraphCallAsync();
                return Results.Ok(drives);
            } catch (Exception ex) {
                return Results.BadRequest($"Error getting drives: {ex.Message}");
            }
        });
        return group;
    }
}
