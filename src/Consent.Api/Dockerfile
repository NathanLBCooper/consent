FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Consent.Api/Consent.Api.csproj", "src/Consent.Api/"]
COPY ["src/Consent.Api.Client/Consent.Api.Client.csproj", "src/Consent.Api.Client/"]
COPY ["src/Consent.Domain/Consent.Domain.csproj", "src/Consent.Domain/"]
COPY ["src/Consent.Storage/Consent.Storage.csproj", "src/Consent.Storage/"]
COPY ["src/Consent.Application/Consent.Application.csproj", "src/Consent.Application/"]
RUN dotnet restore "src/Consent.Api/Consent.Api.csproj"
COPY . .
WORKDIR "/src/src/Consent.Api"
RUN dotnet build "Consent.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Consent.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consent.Api.dll"]
