namespace InmobarcoDocsAPI.Core;
using Azure.Identity;
using InmobarcoDocsAPI.Config;
using Microsoft.Graph;
using Microsoft.Graph.Drives.Item.Items.Item.Copy;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Http.HttpClientLibrary.Middleware.Options;
using System.IO;

public class GraphHelper {
    // Settings object
    private static Settings? _settings;
    // App-ony auth token credential
    private static ClientSecretCredential? _clientSecretCredential;
    // Client configured with app-only authentication
    private static GraphServiceClient? _appClient;
    public static Dictionary<string, string> templates { get; set; } = [];
    private static string baseFolder = "FOLDERS";
    public static void InitializeGraphForAppOnlyAuth(Settings settings) {
        _settings = settings;

        // Ensure settings isn't null
        _ = settings ??
            throw new System.NullReferenceException("Settings cannot be null");

        _settings = settings;

        if (_clientSecretCredential == null) {
            _clientSecretCredential = new ClientSecretCredential(
                _settings.TenantId, _settings.ClientId, _settings.ClientSecret);
        }

        if (_appClient == null) {
            _appClient = new GraphServiceClient(_clientSecretCredential,
                // Use the default scope, which will request the scopes
                // configured on the app registration
                new[] { "https://graph.microsoft.com/.default" });
        }
        _ = GetTemplatesAsync();
    }

    public async static Task<DriveItemCollectionResponse> GetTemplatesAsync() {
        // Ensure client isn't null
        _ = _appClient ??
            throw new System.NullReferenceException("Graph has not been initialized for app-only auth");

        var result = await _appClient.Drives[_settings.driveId].Items[_settings.templatesFolderId].Children.GetAsync((requestConfiguration) => {
            requestConfiguration.QueryParameters.Select = new string[] { "id", "name" }; ;
        });
        templates.Clear();
        foreach (var item in result.Value) {
            if (item.Name == null || item.Id == null) continue;
            templates.Add(item.Name, item.Id);
        }
        return result;
    }
    public async static Task<DriveItemCollectionResponse> GetContractFoldersAsync(string? folderId = "") {
        // Ensure client isn't null
        _ = _appClient ??
            throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
        if (folderId == "") folderId = _settings.apiFolderId;
        var result = await _appClient.Drives[_settings.driveId].Items[folderId].Children.GetAsync((requestConfiguration) => {
            requestConfiguration.QueryParameters.Select = new string[] { "id", "name" }; ;
        });
        return result;
    }
    public async static Task<string> GetItemIdAsync(string itemName, string folderId = "") {
        var items = await GetContractFoldersAsync(folderId);
        string? itemId = "";
        foreach (var item in items.Value) {
            if (item.Name.ToUpper() == itemName.ToUpper()) {
                itemId = item.Id;
                break;
            }
        }
        return itemId;
    }
    public async static Task<Stream> GetFileAsync(string fileId) {
        // Ensure client isn't null
        _ = _appClient ??
            throw new System.NullReferenceException("Graph has not been initialized for app-only auth");

        var result = await _appClient.Drives[_settings.driveId].Items[fileId].Content.GetAsync();
        return result;
    }
    public async static Task<string> CreateFolderAsync(string newFolderName) {
        // Ensure client isn't null
        _ = _appClient ??
            throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
        var requestBody = new CopyPostRequestBody {
            ParentReference = new ItemReference {
                DriveId = _settings.driveId,
                Id = _settings.apiFolderId,
            },
            Name = newFolderName.ToUpper(),
        };
        var headersInspectionHandlerOption = new HeadersInspectionHandlerOption() {
            InspectResponseHeaders = true // specific you wish to collect reponse headers
        };
        string? baseFolderId = "";
        templates.TryGetValue(baseFolder, out baseFolderId);
        await _appClient.Drives[_settings.driveId].Items[baseFolderId].Copy.PostAsync(requestBody, requestConfiguration => requestConfiguration.Options.Add(headersInspectionHandlerOption));
        string result = await GetItemIdAsync(newFolderName);
        return result;
    }
    public async static Task<DriveItem> SaveFile(string folderId, MemoryStream file, string fileName) {
        // Ensure client isn't null
        _ = _appClient ??
            throw new System.NullReferenceException("Graph has not been initialized for app-only auth");
        string? newFileId = await GetItemIdAsync(fileName, folderId);
        var requestBody = new CopyPostRequestBody {
            ParentReference = new ItemReference {
                DriveId = _settings.driveId,
                Id = folderId,
            },
            Name = fileName,
        };
        if (newFileId == "") await _appClient.Drives[_settings.driveId].Items[templates.ElementAt(1).Value].Copy.PostAsync(requestBody); // Copy a file
        newFileId = await GetItemIdAsync(fileName, folderId);
        var result = await _appClient.Drives[_settings.driveId].Items[newFileId].Content.PutAsync(file);// Update the file with the actual content
        return result;
    }

}
