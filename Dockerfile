FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj y restaurar dependencias
COPY ["AstraVenturaNotebook.Api/AstraVenturaNotebook.Api.csproj", "AstraVenturaNotebook.Api/"]
COPY ["AstraVenturaNotebook.Application/AstraVenturaNotebook.Application.csproj", "AstraVenturaNotebook.Application/"]
COPY ["AstraVenturaNotebook.Core/AstraVenturaNotebook.Core.csproj", "AstraVenturaNotebook.Core/"]
COPY ["AstraVenturaNotebook.Infrastructure/AstraVenturaNotebook.Infrastructure.csproj", "AstraVenturaNotebook.Infrastructure/"]
RUN dotnet restore "AstraVenturaNotebook.Api/AstraVenturaNotebook.Api.csproj"

# Copiar el resto del código y compilar
COPY . .
WORKDIR "/src/AstraVenturaNotebook.Api"
RUN dotnet build "AstraVenturaNotebook.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AstraVenturaNotebook.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AstraVenturaNotebook.Api.dll"]
