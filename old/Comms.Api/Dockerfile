FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
# ENV ASPNETCORE_URLS=http://+:80
WORKDIR /app
EXPOSE 80
EXPOSE 81

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS develop
# ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /src/Comms.Api
EXPOSE 80
EXPOSE 81

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Comms.Api/Comms.Api.csproj", "Comms.Api/"]
RUN dotnet restore "Comms.Api/Comms.Api.csproj"
COPY . .
WORKDIR "/src/Comms.Api"
RUN dotnet build "Comms.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Comms.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Comms.Api.dll"]
