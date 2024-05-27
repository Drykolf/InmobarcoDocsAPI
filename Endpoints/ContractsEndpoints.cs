using InmobarcoDocsAPI.Config;
using InmobarcoDocsAPI.Core;
using InmobarcoDocsAPI.Dtos;


namespace InmobarcoDocsAPI.Endpoints;

public static class ContractsEndpoints {
    private static string tenantTemplateId = "01QJDHBV2NCOE6B3AXNFCLGSGS4YC34NTX";
    // Settings object
    private static Settings? _settings;
    private static string tenantContractTemplate = "CTO VIVIENDA INMOBARCO SAS.docx";
    public static RouteGroupBuilder MapContractsEndpoints(this WebApplication app, Settings settings) {
        _settings = settings;
        var group = app.MapGroup("/contracts").WithParameterValidation();
        group.MapGet("/", () => "Contracts");

        group.MapPost("/tenant", async (CreateContractTenantDto newContract) => {
            ContractTenant contract = new(newContract.id, newContract.version, newContract.flat, newContract.complex, newContract.utilityRoom, newContract.garage, newContract.address,
                                            newContract.price, newContract.insurance, newContract.duration, newContract.startDate, newContract.endDate, newContract.tenantName,
                                            newContract.tenantId, newContract.tenantPhone, newContract.tenantEmail, newContract.codebtorName, newContract.codebtorId,
                                            newContract.codebtorPhone, newContract.codebtorEmail, newContract.payDay);
            string fileType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            string fileName = contract.GetContractFileName();
            string folderName = contract.GetFolderName();
            string templateId = GraphHelper.templates[tenantContractTemplate];
            string folderId = await GraphHelper.GetItemIdAsync(folderName);
            if (folderId == "") folderId = await GraphHelper.CreateFolderAsync(folderName);
            var contractFile = contract.GenerateContract(templateId);
            using MemoryStream ms = new(contractFile);
            await GraphHelper.SaveFile(folderId, ms, fileName);
            ms.Dispose();
            return Results.File(contractFile, fileType, fileName);
        });

        group.MapPost("/landlord", () => "TODO Landlord contract");
        return group;
    }
}
