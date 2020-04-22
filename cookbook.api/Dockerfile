FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
ENV ASPNETCORE_URLS=http://+:80
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS develop
ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /src/CookBook.Api
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["CookBook.Api/CookBook.Api.csproj", "CookBook.Api/"]
RUN dotnet restore "CookBook.Api/CookBook.Api.csproj"
COPY . .
WORKDIR "/src/CookBook.Api"
RUN dotnet build "CookBook.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CookBook.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CookBook.Api.dll"]
