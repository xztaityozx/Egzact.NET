FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Egzact.Cli/Egzact.Cli.csproj", "src/Egzact.Cli/"]
RUN dotnet restore "src/Egzact.Cli/Egzact.Cli.csproj"
COPY . .
WORKDIR "/src/src/Egzact.Cli"
RUN dotnet build "Egzact.Cli.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Egzact.Cli.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Egzact.Cli.dll"]
