FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Business/Business.csproj", "Business/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Entities/Entities.csproj", "Entities/"]
RUN dotnet restore "WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]

#WORKDIR /src/WebApi
#FROM couchbase:latest
#COPY /configure-node.sh /opt/couchbase/var
#CMD ["/opt/couchbase/var/configure-node.sh"]


#dotnet dev-certs https --trust
#dotnet dev-certs https --clean
#dotnet dev-certs https --verbose

#CREATE INDEX ix_type ON TodoApp(type)

#docker run -d --name db -p 8091-8094:8091-8094 -p 11210:11210 couchbase
#docker build -f WebApi/Dockerfile -t burakhayirli/dotnetcoretodowebapi .
#docker run -d -p 5000:80 --name c_dotnetcoretodowebapi burakhayirli/dotnetcoretodowebapi