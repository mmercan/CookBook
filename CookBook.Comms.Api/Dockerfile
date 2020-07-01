FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
ENV ASPNETCORE_URLS=http://+:80
WORKDIR /app
EXPOSE 80
EXPOSE 81

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS develop
ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /src/CookBook.Comms.Api
EXPOSE 80
EXPOSE 81

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["CookBook.Comms.Api/CookBook.Comms.Api.csproj", "CookBook.Comms.Api/"]
RUN dotnet restore "CookBook.Comms.Api/CookBook.Comms.Api.csproj"
COPY . .
WORKDIR "/src/CookBook.Comms.Api"
RUN dotnet build "CookBook.Comms.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CookBook.Comms.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CookBook.Comms.Api.dll"]
