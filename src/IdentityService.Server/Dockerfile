FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/IdentityService.Server/IdentityService.Server.csproj", "src/IdentityService.Server/"]
RUN dotnet restore "src/IdentityService.Server/IdentityService.Server.csproj"
COPY . .
WORKDIR "/src/src/IdentityService.Server"
RUN dotnet build "IdentityService.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityService.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityService.Server.dll"]