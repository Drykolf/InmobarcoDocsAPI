using InmobarcoDocsAPI.Dtos;
using InmobarcoDocsAPI.Models;
namespace InmobarcoDocsAPI.Endpoints;

public static class ContractsEndpoints {
    public static RouteGroupBuilder MapContractsEndpoints(this WebApplication app) {
        var group = app.MapGroup("/contracts").WithParameterValidation();

        group.MapPost("/tenant", (CreateContractTenantDto newContract) => {
            Contract_Tenant contract = new(newContract.id, newContract.flat, newContract.complex, newContract.utilityRoom, newContract.garage, newContract.address,
                                            newContract.price, newContract.insurance, newContract.duration, newContract.startDate, newContract.endDate, newContract.tenantName,
                                            newContract.tenantId, newContract.tenantPhone, newContract.tenantEmail, newContract.codebtorName, newContract.codebtorId,
                                            newContract.codebtorPhone, newContract.codebtorEmail, newContract.payDay);
            string fileType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            string fileName = contract.GetContractFileName() + ".docx";
            return Results.File(contract.GenerateContract(), fileType, fileName);
        });

        return group;
    }
}
