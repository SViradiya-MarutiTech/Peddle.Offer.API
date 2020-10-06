
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WebApi/Peddle.Offer.Api.csproj", "WebApi/"]
COPY ["Infrastructure/Peddle.Offer.Infrastructure.csproj", "Infrastructure/"]
COPY ["Application/Peddle.Offer.Application.csproj", "Application/"]
COPY ["Domain/Peddle.Offer.Domain.csproj", "Domain/"]
RUN dotnet restore "WebApi/Peddle.Offer.Api.csproj"
COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build "Peddle.Offer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Peddle.Offer.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Peddle.Offer.Api.dll"]