FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY "Payments/KShop.Payments.WebApi/KShop.Payments.WebApi.csproj" "Payments/KShop.Payments.WebApi/"
RUN dotnet restore "Payments/KShop.Payments.WebApi/KShop.Payments.WebApi.csproj"
COPY . .
RUN dotnet build "/src/Payments/KShop.Payments.WebApi/KShop.Payments.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/src/Payments/KShop.Payments.WebApi/KShop.Payments.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KShop.Payments.WebApi.dll"]