# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar csproj y restaurar dependencias
COPY UrlShortener/*.csproj ./UrlShortener/
RUN dotnet restore UrlShortener/UrlShortener.csproj

# Copiar el resto del código y compilar
COPY . .
RUN dotnet publish UrlShortener/UrlShortener.csproj -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Exponer el puerto
EXPOSE 80
ENTRYPOINT ["dotnet", "UrlShortener.dll"]
