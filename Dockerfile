FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/api/app/Intive.Patronage2023.Api.csproj", "src/api/app/"]
COPY ["src/modules/example/api/Intive.Patronage2023.Modules.Example.Api.csproj", "src/modules/example/api/"]
COPY ["src/shared/abstractions/Intive.Patronage2023.Shared.Abstractions.csproj", "src/shared/abstractions/"]
COPY ["src/shared/infrastructure/Intive.Patronage2023.Shared.Infrastructure.csproj", "src/shared/infrastructure/"]
COPY ["src/modules/example/application/Intive.Patronage2023.Modules.Example.Application.csproj", "src/modules/example/application/"]
COPY ["src/modules/example/domain/Intive.Patronage2023.Modules.Example.Domain.csproj", "src/modules/example/domain/"]
COPY ["src/modules/example/contracts/Intive.Patronage2023.Modules.Example.Contracts.csproj", "src/modules/example/contracts/"]
COPY ["src/modules/example/infrastructure/Intive.Patronage2023.Modules.Example.Infrastructure.csproj", "src/modules/example/infrastructure/"]
RUN dotnet restore "src/api/app/Intive.Patronage2023.Api.csproj"
COPY . .
WORKDIR "/src/src/api/app"
RUN dotnet build "Intive.Patronage2023.Api.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Intive.Patronage2023.Api.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Intive.Patronage2023.Api.dll"]