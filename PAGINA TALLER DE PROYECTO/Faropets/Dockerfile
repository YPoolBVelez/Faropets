# Development image for the legacy ASP.NET Core 5 project.
# .NET 5 is out of support; plan an upgrade to a supported LTS release.
FROM mcr.microsoft.com/dotnet/sdk:5.0

WORKDIR /src
EXPOSE 8080

# EF Core CLI matching this project's EF Core 5 dependencies.
RUN dotnet tool install --global dotnet-ef --version 5.0.16
ENV PATH="$PATH:/root/.dotnet/tools"

CMD ["dotnet", "watch", "run", "--no-launch-profile", "--urls", "http://0.0.0.0:8080"]
