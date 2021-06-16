FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY "Orders/KShop.Orders.WebApi/KShop.Orders.WebApi.csproj" "Orders/KShop.Orders.WebApi/"
RUN dotnet restore "Orders/KShop.Orders.WebApi/KShop.Orders.WebApi.csproj"
#COPY . .
COPY Orders/ Orders/
COPY Communications/ Communications/
COPY Shared/ Shared/

RUN dotnet build "/src/Orders/KShop.Orders.WebApi/KShop.Orders.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/src/Orders/KShop.Orders.WebApi/KShop.Orders.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KShop.Orders.WebApi.dll"]