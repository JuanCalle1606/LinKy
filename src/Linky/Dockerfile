FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Linky/Linky/Linky.csproj", "src/Linky/Linky/"]
COPY ["src/Linky/Linky.Client/Linky.Client.csproj", "src/Linky/Linky.Client/"]
RUN dotnet restore "src/Linky/Linky/Linky.csproj"
COPY . .
WORKDIR "/src/src/Linky/Linky"
RUN dotnet build "Linky.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Linky.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Linky.dll"]
