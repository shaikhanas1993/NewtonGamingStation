# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files first to leverage Docker layer caching for restore.
COPY NewtonGamingStation.sln ./
COPY src/NewtonGamingStation.Domain/*.csproj src/NewtonGamingStation.Domain/
COPY src/NewtonGamingStation.Application/*.csproj src/NewtonGamingStation.Application/
COPY src/NewtonGamingStation.Infrastructure/*.csproj src/NewtonGamingStation.Infrastructure/
COPY src/NewtonGamingStation.Api/*.csproj src/NewtonGamingStation.Api/

RUN dotnet restore src/NewtonGamingStation.Api/NewtonGamingStation.Api.csproj

# Copy the rest and publish.
COPY src/ src/
RUN dotnet publish src/NewtonGamingStation.Api/NewtonGamingStation.Api.csproj \
    -c Release -o /app/publish /p:UseAppHost=false

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "NewtonGamingStation.Api.dll"]
