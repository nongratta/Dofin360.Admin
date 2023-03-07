FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
WORKDIR /src
COPY ["Dofin360.Admin/Dofin360.Admin.csproj", "src/Dofin360.Admin/"]
COPY ["Dofin360.Admin.Core/Dofin360.Admin.Core.csproj", "src/Dofin360.Admin.Core/"]
COPY ["Dofin360.Admin.Model/Dofin360.Admin.Model.csproj", "src/Dofin360.Admin.Model/"]
COPY ["NuGet.Config", "NuGet.Config"]
RUN dotnet restore --configfile NuGet.Config -nowarn:msb3202,nu1503 "src/Dofin360.Admin/Dofin360.Admin.csproj"

COPY . .
WORKDIR "/src/Dofin360.Admin"
RUN dotnet build "Dofin360.Admin.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Dofin360.Admin.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Dofin360.Admin.dll"]