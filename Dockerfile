FROM mcr.microsoft.com/dotnet/core/sdk:3.1
COPY . /app
WORKDIR /app

RUN ["dotnet", "restore"]
RUN ["dotnet", "build", "src/DungeonTools.Server/DungeonTools.Server.csproj", "--configuration Release"]

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000

ENTRYPOINT ["dotnet", "run", "--project src/DungeonTools.Server/DungeonTools.Server.csproj", "--configuration Release"]
