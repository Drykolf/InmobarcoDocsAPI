using System.Globalization;

namespace InmobarcoDocsAPI.Core;

public class ContractTenant {
    private string? cto;
    public Dictionary<string, object> tenantData { get; set; }
    public ContractTenant(string id, string? version, string flat, string complex, string utilityRoom, string garage, string address, string price, string insurance, string duration,
                            string startDate, string endDate, string tenantName, string tenantId, string tenantPhone, string tenantEmail,
                            string codebtorName, string codebtorId, string codebtorPhone, string codebtorEmail, string payDay) {
        this.tenantData = new Dictionary<string, object>() {
            ["CTO"] = id + version,
            ["APTO"] = flat,
            ["CONJUNTO"] = complex.ToUpper(),
            ["UTIL"] = utilityRoom,
            ["PARQUEO"] = garage,
            ["DIRECCION"] = address,
            ["PRECIO"] = price,
            ["ANTICIPOS"] = insurance,
            ["DURACION"] = duration,
            ["INICIO"] = startDate,
            ["FIN"] = endDate,
            ["NOM_ARREND"] = tenantName,
            ["ID_ARREND"] = tenantId,
            ["CEL_ARREND"] = tenantPhone,
            ["CORREO_ARREND"] = tenantEmail,
            ["NOM_CODEUDOR"] = codebtorName,
            ["ID_CODEUDOR"] = codebtorId,
            ["CEL_CODEUDOR"] = codebtorPhone,
            ["CORREO_CODEUDOR"] = codebtorEmail,
            ["FECHA"] = DateTime.Now.ToString("dd/MMMM/yyyy", CultureInfo.CreateSpecificCulture("es-MX")),
            ["DIA_PAGO"] = payDay
        };
        this.cto = id;
    }
    public string GetFolderName() {
        return $"{tenantData["APTO"] as string} {tenantData["CONJUNTO"] as string} CTO {cto}";
    }
    public string GetContractFileName() {
        string fileName = $"{tenantData["CTO"] as string}-{tenantData["APTO"] as string} {tenantData["CONJUNTO"] as string} CTO VIVIENDA INMOBARCO SAS.docx";
        return fileName;
    }
    public string GetLetterFileName() {
        string fileName = $"{tenantData["APTO"] as string} {tenantData["CONJUNTO"] as string} CARTA DE PRESENTACION INQUILINO.docx";
        return fileName;
    }

    public byte[] GenerateContract(string templateId) {
        using Stream template = GraphHelper.GetFileAsync(templateId).Result;
        using MemoryStream ms = new();
        template.CopyTo(ms);
        using MemoryStream newDoc = new();
        MiniSoftware.MiniWord.SaveAsByTemplate(newDoc, ms.ToArray(), tenantData);
        newDoc.Seek(0, SeekOrigin.Begin);
        return newDoc.ToArray();
    }
    public byte[] GenerateLetter(string templateId) {
        using Stream template = GraphHelper.GetFileAsync(templateId).Result;
        using MemoryStream ms = new();
        template.CopyTo(ms);
        using MemoryStream newDoc = new();
        MiniSoftware.MiniWord.SaveAsByTemplate(newDoc, ms.ToArray(), tenantData);
        newDoc.Seek(0, SeekOrigin.Begin);
        return newDoc.ToArray();
    }
}
