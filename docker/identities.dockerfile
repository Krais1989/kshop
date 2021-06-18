FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY "Identities/KShop.Identities.WebApi/KShop.Identities.WebApi.csproj" "Identities/KShop.Identities.WebApi/"
RUN dotnet restore "Identities/KShop.Identities.WebApi/KShop.Identities.WebApi.csproj"
#COPY . .
COPY Identities/ Identities/
COPY Shared/ Shared/
RUN dotnet build "/src/Identities/KShop.Identities.WebApi/KShop.Identities.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/src/Identities/KShop.Identities.WebApi/KShop.Identities.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KShop.Identities.WebApi.dll"]