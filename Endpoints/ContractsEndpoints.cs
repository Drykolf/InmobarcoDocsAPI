using InmobarcoDocsAPI.Core;
using InmobarcoDocsAPI.Dtos;


namespace InmobarcoDocsAPI.Endpoints;

public static class ContractsEndpoints {
    public static RouteGroupBuilder MapContractsEndpoints(this WebApplication app) {
        var group = app.MapGroup("/contracts").WithParameterValidation();
        group.MapGet("/", () => "Contracts");
        group.MapPost("/tenant", async (CreateContractTenantDto newContract, IConfiguration config) => {
            ContractTenant contract = new(newContract.id, newContract.flat, newContract.complex, newContract.utilityRoom, newContract.garage, newContract.address,
                                            newContract.price, newContract.insurance, newContract.duration, newContract.startDate, newContract.endDate, newContract.tenantName,
                                            newContract.tenantId, newContract.tenantPhone, newContract.tenantEmail, newContract.codebtorName, newContract.codebtorId,
                                            newContract.codebtorPhone, newContract.codebtorEmail, newContract.payDay);
            string fileType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            string fileName = contract.GetContractFileName() + ".docx";
            string? connString = config["ConnectionStrings:AzureStorage"];
            if (connString == null) return Results.BadRequest("Connection string not found");
            var template = await contract.GetTemplate(connString);
            if (template == null) return Results.NotFound();
            return Results.File(contract.GenerateContract(template), fileType, fileName);
        });

        group.MapPost("/landlord", () => "TODO Landlord contract");
        return group;
    }
}
