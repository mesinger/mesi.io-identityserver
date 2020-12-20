FROM mcr.microsoft.com/dotnet/aspnet:5.0

WORKDIR app
COPY app .

EXPOSE 5000

VOLUME logs/

ENTRYPOINT ["dotnet", "is4.dll", "--urls", "http://0.0.0.0:5000"]