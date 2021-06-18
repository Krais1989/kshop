FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY 'KShop.sln' 'KShop.sln'
#COPY *.csproj Carts/*
COPY "Carts/KShop.Carts.WebApi/KShop.Carts.WebApi.csproj" "Carts/KShop.Carts.WebApi/"
RUN dotnet restore "Carts/KShop.Carts.WebApi/KShop.Carts.WebApi.csproj"

#COPY . .
COPY Carts/ Carts/
COPY Shared/ Shared/
RUN dotnet build "/src/Carts/KShop.Carts.WebApi/KShop.Carts.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/src/Carts/KShop.Carts.WebApi/KShop.Carts.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KShop.Carts.WebApi.dll"]