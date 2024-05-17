using InmobarcoDocsAPI.Core;
using InmobarcoDocsAPI.Dtos;

namespace InmobarcoDocsAPI.Endpoints;

public static class ContractsEndpoints {
    public static RouteGroupBuilder MapContractsEndpoints(this WebApplication app) {
        var group = app.MapGroup("/contracts").WithParameterValidation();
        group.MapGet("/", (IConfiguration config) =>
            config["Templates:TenantContractTemplate"]
        );
        group.MapPost("/tenant", (CreateContractTenantDto newContract, IConfiguration config) => {
            ContractTenant contract = new(newContract.id, newContract.flat, newContract.complex, newContract.utilityRoom, newContract.garage, newContract.address,
                                            newContract.price, newContract.insurance, newContract.duration, newContract.startDate, newContract.endDate, newContract.tenantName,
                                            newContract.tenantId, newContract.tenantPhone, newContract.tenantEmail, newContract.codebtorName, newContract.codebtorId,
                                            newContract.codebtorPhone, newContract.codebtorEmail, newContract.payDay);
            string fileType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            string fileName = contract.GetContractFileName() + ".docx";
            string? templatePath = config["Templates:TenantContractTemplate"] ?? throw new Exception("Template path not found");
            return Results.File(contract.GenerateContract(templatePath), fileType, fileName);
        });

        group.MapPost("/landlord", () => "TODO Landlord contract");
        return group;
    }
}
