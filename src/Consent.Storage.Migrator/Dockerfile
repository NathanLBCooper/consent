#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Consent.Storage.Migrator/Consent.Storage.Migrator.csproj", "src/Consent.Storage.Migrator/"]
RUN dotnet restore "src/Consent.Storage.Migrator/Consent.Storage.Migrator.csproj"
COPY . .
WORKDIR "/src/src/Consent.Storage.Migrator"
RUN dotnet build "Consent.Storage.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Consent.Storage.Migrator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consent.Storage.Migrator.dll"]
