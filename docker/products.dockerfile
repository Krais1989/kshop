FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY "Products/KShop.Products.WebApi/KShop.Products.WebApi.csproj" "Products/KShop.Products.WebApi/"
RUN dotnet restore "Products/KShop.Products.WebApi/KShop.Products.WebApi.csproj"
COPY . .
RUN dotnet build "/src/Products/KShop.Products.WebApi/KShop.Products.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/src/Products/KShop.Products.WebApi/KShop.Products.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KShop.Products.WebApi.dll"]