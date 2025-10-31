# STAGE 1: build/publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas csproj (melhora cache)
COPY ContractService/Contract.Domain/Contract.Domain.csproj ContractService/Contract.Domain/
COPY ContractService/Contract.Application/Contract.Application.csproj ContractService/Contract.Application/
COPY ContractService/Contract.Application.Contracts/Contract.Application.Contracts.csproj ContractService/Contract.Application.Contracts/
COPY ContractService/Contract.Infrastructure/Contract.Infrastructure.csproj ContractService/Contract.Infrastructure/
COPY ContractService/Contract.Api/Contract.Api.csproj ContractService/Contract.Api/
COPY Common/Shared.Contracts/Shared.Contracts.csproj Common/Shared.Contracts/

RUN dotnet restore ContractService/Contract.Api/Contract.Api.csproj

# Copia o restante do código
COPY ContractService/Contract.Domain/ ContractService/Contract.Domain/
COPY ContractService/Contract.Application/ ContractService/Contract.Application/
COPY ContractService/Contract.Application.Contracts/ ContractService/Contract.Application.Contracts/
COPY ContractService/Contract.Infrastructure/ ContractService/Contract.Infrastructure/
COPY ContractService/Contract.Api/ ContractService/Contract.Api/
COPY Common/Shared.Contracts/ Common/Shared.Contracts/

RUN dotnet publish ContractService/Contract.Api/Contract.Api.csproj -c Release -o /app

# STAGE 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8081
ENTRYPOINT ["dotnet", "Contract.Api.dll"]
