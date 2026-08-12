FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/API/API.csproj", "src/API/"]
COPY ["src/Common/Test.Common.Application/Test.Common.Application.csproj", "src/Common/Test.Common.Application/"]
COPY ["src/Common/Test.Common.Domain/Test.Common.Domain.csproj", "src/Common/Test.Common.Domain/"]
COPY ["src/Common/Test.Common.Infrastructure/Test.Common.Infrastructure.csproj", "src/Common/Test.Common.Infrastructure/"]
COPY ["src/Common/Test.Common.Presentation/Test.Common.Presentation.csproj", "src/Common/Test.Common.Presentation/"]
COPY ["src/Modules/Playlists/Module.Playlist.Application/Module.Playlist.Application.csproj", "src/Modules/Playlists/Module.Playlist.Application/"]
COPY ["src/Modules/Playlists/Module.Playlist.Domain/Module.Playlist.Domain.csproj", "src/Modules/Playlists/Module.Playlist.Domain/"]
COPY ["src/Modules/Playlists/Module.Playlist.Infrastructure/Module.Playlist.Infrastructure.csproj", "src/Modules/Playlists/Module.Playlist.Infrastructure/"]
COPY ["src/Modules/Playlists/Module.Playlist.Presentation/Module.Playlist.Presentation.csproj", "src/Modules/Playlists/Module.Playlist.Presentation/"]
COPY ["src/Modules/Songs/Module.Songs.Application/Module.Songs.Application.csproj", "src/Modules/Songs/Module.Songs.Application/"]
COPY ["src/Modules/Songs/Module.Songs.Domain/Module.Songs.Domain.csproj", "src/Modules/Songs/Module.Songs.Domain/"]
COPY ["src/Modules/Songs/Module.Songs.Infrastructure/Module.Songs.Infrastructure.csproj", "src/Modules/Songs/Module.Songs.Infrastructure/"]
COPY ["src/Modules/Songs/Module.Songs.Presentation/Module.Songs.Presentation.csproj", "src/Modules/Songs/Module.Songs.Presentation/"]
COPY ["src/Modules/Users/Module.Users.Application/Module.Users.Application.csproj", "src/Modules/Users/Module.Users.Application/"]
COPY ["src/Modules/Users/Module.Users.Domain/Module.Users.Domain.csproj", "src/Modules/Users/Module.Users.Domain/"]
COPY ["src/Modules/Users/Module.Users.Infrastructure/Module.Users.Infrastructure.csproj", "src/Modules/Users/Module.Users.Infrastructure/"]
COPY ["src/Modules/Users/Module.Users.Presentation/Module.Users.Presentation.csproj", "src/Modules/Users/Module.Users.Presentation/"]

RUN dotnet restore "src/API/API.csproj"

COPY . .
WORKDIR /src/src/API
RUN dotnet build "API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
