FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY "Shipments/KShop.Shipments.WebApi/KShop.Shipments.WebApi.csproj" "Shipments/KShop.Shipments.WebApi/"
RUN dotnet restore "Shipments/KShop.Shipments.WebApi/KShop.Shipments.WebApi.csproj"
COPY . .
RUN dotnet build "/src/Shipments/KShop.Shipments.WebApi/KShop.Shipments.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/src/Shipments/KShop.Shipments.WebApi/KShop.Shipments.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KShop.Shipments.WebApi.dll"]