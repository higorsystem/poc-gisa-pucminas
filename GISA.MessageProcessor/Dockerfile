#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM higormendes/gisa_aspnetcorehelper:latest AS base
WORKDIR /app
EXPOSE 80
LABEL MAINTAINER="jorge.higor@gmail.com"

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["GISA.MessageProcessor/GISA.MessageProcessor.csproj", "GISA.MessageProcessor/"]
COPY ["GISA.Domain/GISA.Domain.csproj", "GISA.Domain/"]
COPY ["GISA.Commons.IoC/GISA.Commons.IoC.csproj", "GISA.Commons.IoC/"]
COPY ["GISA.MIC.Repository/GISA.MIC.Repository.csproj", "GISA.MIC.Repository/"]
RUN dotnet restore "GISA.MessageProcessor/GISA.MessageProcessor.csproj"
COPY . .
WORKDIR "/src/GISA.MessageProcessor"
RUN dotnet build "GISA.MessageProcessor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GISA.MessageProcessor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY ./dapr /dapr