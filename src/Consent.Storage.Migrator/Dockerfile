FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Consent.Storage.Migrator/Consent.Storage.Migrator.csproj", "src/Consent.Storage.Migrator/"]
RUN dotnet restore "src/Consent.Storage.Migrator/Consent.Storage.Migrator.csproj"
COPY . .
WORKDIR "/src/src/Consent.Storage.Migrator"
RUN dotnet build "Consent.Storage.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Consent.Storage.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consent.Storage.Migrator.dll"]
