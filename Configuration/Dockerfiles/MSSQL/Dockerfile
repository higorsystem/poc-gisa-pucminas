FROM mcr.microsoft.com/mssql/server:2019-latest

LABEL VERSION="1.0"
LABEL AUTHOR="Jorge Higor Mendes dos Santos"
LABEL Purpose="Custom SQL 2019 Image with Wait Utility"

EXPOSE 1433

## Add the wait script to the image
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.9.0/wait /wait
USER root
RUN chmod +x /wait