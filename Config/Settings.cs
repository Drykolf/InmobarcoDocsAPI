namespace InmobarcoDocsAPI.Config;
using Microsoft.Extensions.Configuration;

public class Settings {
    public string? ClientId { get; set; } //Micosoft App ID
    public string? ClientSecret { get; set; } //Microsoft App Secret
    public string? TenantId { get; set; } //Organization ID
    public string? driveId { get; set; }//Documents Library ID
    public string? apiFolderId { get; set; }//Documents Folder ID for the API to use
    public string? templatesFolderId { get; set; }//Templates Folder ID 
    public static Settings LoadSettings() {
        // Load settings
        IConfiguration config = new ConfigurationBuilder()
            // appsettings.json is required
            .AddJsonFile("appsettings.json", optional: false)
            // appsettings.Development.json" is optional, values override appsettings.json
            .AddJsonFile($"appsettings.Development.json", optional: true)
            // User secrets are optional, values override both JSON files
            .AddUserSecrets<Program>()
            .Build();

        return config.GetRequiredSection("Settings").Get<Settings>() ??
            throw new Exception("Could not load app settings. See README for configuration instructions.");
    }
}