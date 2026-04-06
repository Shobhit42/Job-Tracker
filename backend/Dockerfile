# Stage 1 — Build
# Use the official .NET 9 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files first
# This allows Docker to cache the restore layer separately
COPY ["JobTracker.API/JobTracker.API.csproj", "JobTracker.API/"]
COPY ["JobTracker.Application/JobTracker.Application.csproj", "JobTracker.Application/"]
COPY ["JobTracker.Infrastructure/JobTracker.Infrastructure.csproj", "JobTracker.Infrastructure/"]
COPY ["JobTracker.Domain/JobTracker.Domain.csproj", "JobTracker.Domain/"]

# Restore NuGet packages
RUN dotnet restore "JobTracker.API/JobTracker.API.csproj"

# Copy all source code
COPY . .

# Build and publish the API in Release mode
RUN dotnet publish "JobTracker.API/JobTracker.API.csproj" -c Release -o /app/publish

# Stage 2 — Runtime
# Use the smaller runtime-only image to run the app
# SDK image is only needed to build, not to run
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Tell ASP.NET Core to listen on port 8080
# Render expects your app to listen on this port
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "JobTracker.API.dll"]