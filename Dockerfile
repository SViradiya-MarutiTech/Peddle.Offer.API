
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /source
COPY ["/src/WebApi/Api.csproj", "WebApi/"]
COPY ["/src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["/src/Application/Application.csproj", "Application/"]
COPY ["/src/Domain/Domain.csproj", "Domain/"]

COPY ["NuGet.Config","NuGet.Config"]

COPY /src/WebApi WebApi/
COPY /src/Infrastructure Infrastructure/
COPY /src/Application Application/
COPY /src/Domain Domain/

COPY packages packages

RUN dotnet restore "/source/WebApi/Api.csproj"

WORKDIR "/source/WebApi"
RUN dotnet build "Api.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]