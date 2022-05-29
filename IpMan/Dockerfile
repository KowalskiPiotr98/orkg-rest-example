FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["IpMan/IpMan.csproj", "IpMan/"]
RUN dotnet restore "IpMan/IpMan.csproj"
COPY . .
WORKDIR "/src/IpMan"
RUN dotnet build "IpMan.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IpMan.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IpMan.dll"]
