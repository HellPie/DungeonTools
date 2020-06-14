FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

COPY . ./
RUN dotnet publish ./src/DungeonTools.Server/DungeonTools.Server.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /out

COPY --from=build /out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "DungeonTools.Server.dll"]
