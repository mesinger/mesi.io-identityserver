FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR app
COPY app .

EXPOSE 5000

VOLUME logs/

ENTRYPOINT ["dotnet", "is4.dll", "--urls", "http://0.0.0.0:5000"]