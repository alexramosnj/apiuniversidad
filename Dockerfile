# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "ApiUniversidad.csproj"
RUN dotnet publish "ApiUniversidad.csproj" -c Release -o /app

# --- Runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
# Render asigna el puerto con $PORT, así que lo exponemos así:
CMD ["sh", "-c", "ASPNETCORE_URLS=http://0.0.0.0:$PORT dotnet ApiUniversidad.dll"]
