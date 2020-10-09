
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["/src/WebApi/Api.csproj", "WebApi/"]
COPY ["/src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["/src/Application/Application.csproj", "Application/"]
COPY ["/src/Domain/Domain.csproj", "Domain/"]
COPY ["NuGet.Config","NuGet.Config"]

RUN dotnet restore "/src/WebApi/Api.csproj"
COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]