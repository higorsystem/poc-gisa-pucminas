#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM gisa_aspnetcorehelper:latest AS base
WORKDIR /app
EXPOSE 80

LABEL MAINTAINER="jorge.higor@gmail.com"

## Arguments required on build time
#ARG AWS_REGION
#ARG AWS_PROFILE
#ARG AWS_SECRET_ACCESS_KEY
#ARG AWS_ACCESS_KEY_ID
#
## Forge input arguments into permanent env variables
#ENV AWS_REGION=${AWS_REGION}
#ENV AWS_PROFILE=${AWS_PROFILE}
#ENV AWS_SECRET_ACCESS_KEY=${AWS_SECRET_ACCESS_KEY}
#ENV AWS_ACCESS_KEY_ID=${AWS_ACCESS_KEY_ID}

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["GISA.Authentication/GISA.Authentication.WebApi/GISA.Authentication.WebApi.csproj", "GISA.Authentication/GISA.Authentication.WebApi/"]
COPY ["GISA.Commons.SDK/GISA.Commons.SDK.csproj", "GISA.Commons.SDK/"]
COPY ["GISA.Commons.IoC/GISA.Commons.IoC.csproj", "GISA.Commons.IoC/"]
COPY ["GISA.Authentication/GISA.Authentication.Application/GISA.Authentication.Application.csproj", "GISA.Authentication/GISA.Authentication.Application/"]
COPY ["GISA.Authentication/GISA.Authentication.Domain/GISA.Authentication.Domain.csproj", "GISA.Authentication/GISA.Authentication.Domain/"]
COPY ["GISA.Authentication/GISA.Authentication.Infra.IoC/GISA.Authentication.Infra.IoC.csproj", "GISA.Authentication/GISA.Authentication.Infra.IoC/"]
RUN dotnet restore "GISA.Authentication/GISA.Authentication.WebApi/GISA.Authentication.WebApi.csproj"
COPY . .
WORKDIR "/src/GISA.Authentication/GISA.Authentication.WebApi"
RUN dotnet build "GISA.Authentication.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GISA.Authentication.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GISA.Authentication.WebApi.dll"]

COPY ./dapr dapr