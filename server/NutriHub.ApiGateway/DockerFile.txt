FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NutriHub.ApiGateway/NutriHub.ApiGateway.csproj", "NutriHub.ApiGateway/"]
RUN dotnet restore "NutriHub.ApiGateway/NutriHub.ApiGateway.csproj"
COPY . .
WORKDIR "/src/NutriHub.ApiGateway"
RUN dotnet build "NutriHub.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NutriHub.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NutriHub.ApiGateway.dll"]