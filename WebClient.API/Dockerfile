FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CleanCommerce.sln", "."]
COPY ["WebClient.API/WebClient.API.csproj", "WebClient.API/"]
COPY ["WebClient.Application/WebClient.Application.csproj", "WebClient.Application/"]
COPY ["WebClient.Domain/WebClient.Domain.csproj", "WebClient.Domain/"]
COPY ["WebClient.Infrastructure/WebClient.Infrastructure.csproj", "WebClient.Infrastructure/"]
RUN dotnet restore "WebClient.API/WebClient.API.csproj"
COPY . .
WORKDIR "/src/WebClient.API"
RUN dotnet build "WebClient.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebClient.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebClient.API.dll"]

EXPOSE 80
