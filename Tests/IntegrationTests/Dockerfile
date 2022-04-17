#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM rrvieira/core-sdk-selenium:1.0 AS build
LABEL MAINTAINER="jorge.higor@gmail.com"
USER seluser

WORKDIR /src

COPY --chown=seluser ["Tests/IntegrationTests/**", "Tests/IntegrationTests/"]
COPY --chown=seluser ["GISA.Domain/**", "GISA.Domain/"]
COPY --chown=seluser ["GISA.MIC.Repository/**", "GISA.MIC.Repository/"]

RUN dotnet restore "Tests/IntegrationTests/IntegrationTests.csproj"