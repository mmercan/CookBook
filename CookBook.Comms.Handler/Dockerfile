FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
# ENV ASPNETCORE_URLS=http://+:80
WORKDIR /app
EXPOSE 80
EXPOSE 81

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS develop
# ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /src/CookBook.Comms.Handler
EXPOSE 80
EXPOSE 81

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["CookBook.Comms.Handler/CookBook.Comms.Handler.csproj", "CookBook.Comms.Handler/"]
RUN dotnet restore "CookBook.Comms.Handler/CookBook.Comms.Handler.csproj"
COPY . .
WORKDIR "/src/CookBook.Comms.Handler"
RUN dotnet build "CookBook.Comms.Handler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CookBook.Comms.Handler.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CookBook.Comms.Handler.dll"]
