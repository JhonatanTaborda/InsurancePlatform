# Plataforma de Seguros

Dois serviços: **Proposal** e **Contract**. Comunicação assíncrona via RabbitMQ. Persistência em SQL Server com EF Core e migrations. Testes com xUnit e Moq.


## Pré-requisitos
Docker Desktop (com WSL 2 habilitado), .NET 8 SDK (se rodar local), EF Core Tools.


## Subir tudo
```
# na raiz do repositório (onde está o docker-compose.yml)
docker compose up -d
docker compose ps
```
RabbitMQ UI: http://localhost:15672 (guest/guest). Proposal API: http://localhost:8080/swagger, Contract API: http://localhost:8081/swagger.


## Migrations
Dentro do repo de cada serviço:
```
#Proposal
cd ProposalService/Proposal.Infrastructure
$env:ConnectionStrings__Sql = "Server=localhost,14333;Database=ProposalDb;User Id=sa;Password=Str0ng!Passw0rd#;TrustServerCertificate=True"
# dotnet ef migrations add Init -s ../Proposal.Api -c Proposal.Infrastructure.Entity.ProposalDbContext
dotnet ef database update -s ../Proposal.Api -c Proposal.Infrastructure.Entity.ProposalDbContext
Remove-Item Env:\ConnectionStrings__Sql

#Contracts
cd ../../ContractService/Contract.Infrastructure
$env:ConnectionStrings__Sql = "Server=localhost,14333;Database=ContractDb;User Id=sa;Password=Str0ng!Passw0rd#;TrustServerCertificate=True"
# dotnet ef migrations add Init -s ../Contract.Api -c Contract.Infrastructure.Entity.ContractDbContext --verbose
dotnet ef database update -s ../Contract.Api -c Contract.Infrastructure.Entity.ContractDbContext
Remove-Item Env:\ConnectionStrings__Sql
```

## Fluxo
1. Criar proposta
```
POST http://localhost:8080/api/proposals
{
"customerId": "<guid>",
"product": "Auto",
"premium": 120.50
}
```
2. Aprovar proposta
```
PATCH http://localhost:8080/api/proposals/{id}/status
{
"status": "Approved"
}
```
3. Contratar
```
POST http://localhost:8081/api/contracts
{
"proposalId": "{id}"
}
```
A contratação só ocorre se o evento `ProposalApproved` tiver sido consumido pelo serviço de contratação.


## Testes
Testes unitários