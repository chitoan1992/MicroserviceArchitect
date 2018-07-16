FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY InventoryService/InventoryService.csproj InventoryService/
RUN dotnet restore InventoryService/InventoryService.csproj
COPY . .
WORKDIR /src/InventoryService
RUN dotnet build InventoryService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish InventoryService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "InventoryService.dll"]
