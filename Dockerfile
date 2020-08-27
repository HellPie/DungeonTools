FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

COPY . ./
RUN dotnet publish ./src/DungeonTools.Server/DungeonTools.Server.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /out

COPY --from=build /out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "DungeonTools.Server.dll"]
