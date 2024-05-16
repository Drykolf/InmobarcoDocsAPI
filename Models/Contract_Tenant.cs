using System.Globalization;

namespace InmobarcoDocsAPI.Models;

public class Contract_Tenant {
    public Contract_Tenant(string id, string flat, string complex, string utilityRoom, string garage, string address, string price, string insurance, string duration,
                            string startDate, string endDate, string tenantName, string tenantId, string tenantPhone, string tenantEmail,
                            string codebtorName, string codebtorId, string codebtorPhone, string codebtorEmail, int payDay) {
        this.tenantData = new Dictionary<string, object>() {
            ["CTO"] = id,
            ["APTO"] = flat,
            ["CONJUNTO"] = complex,
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
    }
    public Dictionary<string, object> tenantData { get; set; }
    public string GetContractFileName() {
        string fileName = $"{tenantData["CTO"] as string}-{tenantData["APTO"] as string} {tenantData["CONJUNTO"] as string} CTO VIVIENDA INMOBARCO SAS";
        return fileName;
    }
    public string GetLetterFileName() {
        string fileName = $"{tenantData["APTO"] as string} {tenantData["CONJUNTO"] as string} CARTA DE PRESENTACION INQUILINO";
        return fileName;
    }
    public MemoryStream GenerateContract() {
        string directory = "/Contratos/";
        Directory.CreateDirectory(directory);
        string outputDoc = directory + GetContractFileName() + ".docx";
        MemoryStream newDoc = new MemoryStream();
        string templatePath = "0 CTO VIVIENDA INMOBARCO SAS.docx";
        //Do something with your stream here
        MiniSoftware.MiniWord.SaveAsByTemplate(newDoc, templatePath, tenantData);
        newDoc.Seek(0, SeekOrigin.Begin);
        return newDoc;
    }
}
