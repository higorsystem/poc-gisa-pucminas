#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM higormendes/gisa_aspnetcorehelper:latest AS base
WORKDIR /app
EXPOSE 80
LABEL MAINTAINER="jorge.higor@gmail.com"

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["GISA.MIC.WebApi/GISA.MIC.WebApi.csproj", "GISA.MIC.WebApi/"]
COPY ["GISA.MIC.Application/GISA.MIC.Application.csproj", "GISA.MIC.Application/"]
COPY ["GISA.Domain/GISA.Domain.csproj", "GISA.Domain/"]
COPY ["GISA.MIC.Repository/GISA.MIC.Repository.csproj", "GISA.MIC.Repository/"]
COPY ["GISA.Commons.IoC/GISA.Commons.IoC.csproj", "GISA.Commons.IoC/"]
COPY ["GISA.Commons.SDK/GISA.Commons.SDK.csproj", "GISA.Commons.SDK/"]
RUN dotnet restore "GISA.MIC.WebApi/GISA.MIC.WebApi.csproj"
COPY . .
WORKDIR "/src/GISA.MIC.WebApi"
RUN dotnet build "GISA.MIC.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GISA.MIC.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY ./dapr /dapr