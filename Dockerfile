FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

WORKDIR /src
COPY ./Sources/Maintenance/*.csproj ./Sources/Maintenance/
COPY ./Sources/Maintenance.Letstalk/*.csproj ./Sources/Maintenance.Letstalk/
COPY ./Sources/Maintenance.BotService/*.csproj ./Sources/Maintenance.BotService/
COPY ./Sources/Maintenance.Mail/*.csproj ./Sources/Maintenance.Mail/
COPY ./Sources/Maintenance.MailStation/*.csproj ./Maintenance.MailStation/
COPY ./Sources/Maintenance.TraceSp/*.csproj ./Sources/Maintenance.TraceSp/
COPY ./Maintenance.Sites/*.csproj  ./Maintenance.Sites/

RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN --mount=type=cache,target=/root/.nuget/packages \ 
	dotnet restore Maintenance.Sites/Maintenance.Sites.csproj

COPY ./Sources/. ./Sources
COPY ./Maintenance.Sites/. ./Maintenance.Sites

WORKDIR /src/Maintenance.Sites
RUN --mount=type=cache,target=/root/.nuget/packages \
	dotnet publish -c release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine AS base

WORKDIR /app
COPY --from=builder /app .

ENV TZ="America/La_Paz"
ENV COMPlus_EnableDiagnostics=0

# Don t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

ENTRYPOINT ["dotnet", "Maintenance.Sites.dll"]
