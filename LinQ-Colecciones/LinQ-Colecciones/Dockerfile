FROM mcr.microsoft.com/dotnet/runtime:10.0-nanoserver-ltsc2022 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0-nanoserver-ltsc2022 AS build
ARG configuration=Release
WORKDIR /src
COPY ["LinQ-Colecciones/LinQ-Colecciones/LinQ-Colecciones.csproj", "LinQ-Colecciones/LinQ-Colecciones/"]
RUN dotnet restore "LinQ-Colecciones\LinQ-Colecciones\LinQ-Colecciones.csproj"
COPY . .
WORKDIR "/src/LinQ-Colecciones/LinQ-Colecciones"
RUN dotnet build "LinQ-Colecciones.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "LinQ-Colecciones.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LinQ-Colecciones.dll"]
