dapr run --app-id gisa_authentication --app-port 5000 --dapr-http-port 3600 --dapr-grpc-port 60000 --config ../dapr/config/config.yaml --components-path ../dapr dotnet run