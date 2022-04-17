#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM aspnetcorehelper:latest AS base
WORKDIR /app
EXPOSE 80

LABEL MAINTAINER="jorge.higor@gmail.com"

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["GISA.AuthorizationService/GISA.AuthorizationService.csproj", "GISA.AuthorizationService/"]
COPY ["GISA.Domain/GISA.Domain.csproj", "GISA.Domain/"]
RUN dotnet restore "GISA.AuthorizationService/GISA.AuthorizationService.csproj"
COPY . .
WORKDIR "/src/GISA.AuthorizationService"
RUN dotnet build "GISA.AuthorizationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GISA.AuthorizationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY ./dapr /dapr