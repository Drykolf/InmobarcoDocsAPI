﻿namespace InmobarcoDocsAPI.Dtos;
public record class CreateContractTenantDto(
    string id,
    string flat,
    string complex,
    string utilityRoom,
    string garage,
    string address,
    string price,
    string insurance,
    string duration,
    string startDate,
    string endDate,
    string tenantName,
    string tenantId,
    string tenantPhone,
    string tenantEmail,
    string codebtorName,
    string codebtorId,
    string codebtorPhone,
    string codebtorEmail,
    int payDay);
