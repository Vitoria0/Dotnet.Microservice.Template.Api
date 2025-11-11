FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/MicroserviceTemplate.API/MicroserviceTemplate.API.csproj", "src/MicroserviceTemplate.API/"]
COPY ["src/MicroserviceTemplate.Application/MicroserviceTemplate.Application.csproj", "src/MicroserviceTemplate.Application/"]
COPY ["src/MicroserviceTemplate.Domain/MicroserviceTemplate.Domain.csproj", "src/MicroserviceTemplate.Domain/"]
COPY ["src/MicroserviceTemplate.Infrastructure/MicroserviceTemplate.Infrastructure.csproj", "src/MicroserviceTemplate.Infrastructure/"]

RUN dotnet restore "src/MicroserviceTemplate.API/MicroserviceTemplate.API.csproj"
COPY . .
WORKDIR "/src/src/MicroserviceTemplate.API"
RUN dotnet build "MicroserviceTemplate.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceTemplate.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceTemplate.API.dll"]

