# STAGE 1: build/publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar só os .csproj primeiro (aproveita cache)
COPY ProposalService/Proposal.Domain/Proposal.Domain.csproj ProposalService/Proposal.Domain/
COPY ProposalService/Proposal.Application/Proposal.Application.csproj ProposalService/Proposal.Application/
COPY ProposalService/Proposal.Application.Contracts/Proposal.Application.Contracts.csproj ProposalService/Proposal.Application.Contracts/
COPY ProposalService/Proposal.Infrastructure/Proposal.Infrastructure.csproj ProposalService/Proposal.Infrastructure/
COPY ProposalService/Proposal.Api/Proposal.Api.csproj ProposalService/Proposal.Api/

COPY Common/Shared.Contracts/Shared.Contracts.csproj Common/Shared.Contracts/

RUN dotnet restore ProposalService/Proposal.Api/Proposal.Api.csproj

# Copiar o restante do código
COPY ProposalService/Proposal.Domain/ ProposalService/Proposal.Domain/
COPY ProposalService/Proposal.Application/ ProposalService/Proposal.Application/
COPY ProposalService/Proposal.Application.Contracts/ ProposalService/Proposal.Application.Contracts/
COPY ProposalService/Proposal.Infrastructure/ ProposalService/Proposal.Infrastructure/
COPY ProposalService/Proposal.Api/ ProposalService/Proposal.Api/
COPY Common/Shared.Contracts/ Common/Shared.Contracts/

RUN dotnet publish ProposalService/Proposal.Api/Proposal.Api.csproj -c Release -o /app

# STAGE 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Proposal.Api.dll"]
