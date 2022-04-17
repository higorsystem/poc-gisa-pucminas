FROM higormendes/gisa_pythonautomation:latest

LABEL MAINTAINER="jorge.higor@gmail.com"
COPY ["GISA.NotificationService/requirements.txt", "./requirements.txt"]
COPY ["GISA.NotificationService/server.py", "./server.py"]

RUN pip install -r requirements.txt

COPY ./dapr /dapr