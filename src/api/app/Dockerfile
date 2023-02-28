FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/api/app/Intive.Patronage2023.Api.csproj", "src/api/app/"]
RUN dotnet restore "src/api/app/Intive.Patronage2023.Api.csproj"
COPY . .
WORKDIR "src\api\app"
RUN dotnet build "Intive.Patronage2023.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Intive.Patronage2023.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Intive.Patronage2023.Api.dll"]
