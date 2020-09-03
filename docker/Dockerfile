FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR app
COPY app .

EXPOSE 5000

VOLUME logs/

ARG userdb
ARG private
ARG public

ENV UserDatabase__ConnectionString=$userdb
ENV Certificate__Private=$private
ENV Certificate__Public=$public

ENTRYPOINT ["dotnet", "is4.dll", "--urls", "http://0.0.0.0:5000"]