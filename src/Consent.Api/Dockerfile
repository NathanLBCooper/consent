#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Consent.Api/Consent.Api.csproj", "src/Consent.Api/"]
COPY ["src/Consent.Domain/Consent.Domain.csproj", "src/Consent.Domain/"]
COPY ["src/Consent.Storage/Consent.Storage.csproj", "src/Consent.Storage/"]
RUN dotnet restore "src/Consent.Api/Consent.Api.csproj"
COPY . .
WORKDIR "/src/src/Consent.Api"
RUN dotnet build "Consent.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Consent.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consent.Api.dll"]
