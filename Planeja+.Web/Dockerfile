FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Planeja+.slnx .
COPY Planeja+.Domain/Planeja+.Domain.csproj Planeja+.Domain/
COPY Planeja+.Application/Planeja+.Application.csproj Planeja+.Application/
COPY Planeja+.Infrastructure/Planeja+.Infrastructure.csproj Planeja+.Infrastructure/
COPY Planeja+.Web/Planeja+.Web.csproj Planeja+.Web/

RUN dotnet restore "Planeja+.slnx"

COPY . .
RUN dotnet publish "Planeja+.Web/Planeja+.Web.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Planeja+.Web.dll"]
