FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ShippingService/ShippingService.csproj ShippingService/
RUN dotnet restore ShippingService/ShippingService.csproj
COPY . .
WORKDIR /src/ShippingService
RUN dotnet build ShippingService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ShippingService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ShippingService.dll"]
