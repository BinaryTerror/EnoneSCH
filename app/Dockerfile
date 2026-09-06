# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/School.Api/School.Api.csproj", "src/School.Api/"]
COPY ["src/School.Application/School.Application.csproj", "src/School.Application/"]
COPY ["src/School.Domain/School.Domain.csproj", "src/School.Domain/"]
COPY ["src/School.Infrastructure/School.Infrastructure.csproj", "src/School.Infrastructure/"]

RUN dotnet restore "src/School.Api/School.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/School.Api"
RUN dotnet build "School.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "School.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create uploads directory for local video storage (dev fallback)
RUN mkdir -p /app/uploads

EXPOSE 8080
ENTRYPOINT ["dotnet", "School.Api.dll"]