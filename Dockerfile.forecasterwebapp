FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY Forecaster.sln ./
COPY ForecastsBlazor/*.csproj ./ForecastsBlazor/
COPY ForecastsCollector/*.csproj ./ForecastsCollector/
COPY ForecastsRabbitMQProcessor/*.csproj ./ForecastsRabbitMQProcessor/
COPY ForecastsCommon/*.csproj ./ForecastsCommon/
COPY ForecastsConsole/*.csproj ./ForecastsConsole/
COPY ForecastsConsumerConsole/*.csproj ./ForecastsConsumerConsole/

RUN dotnet restore
COPY . .
WORKDIR /src/ForecastsCommon
RUN dotnet build -c Release -o /app

WORKDIR /src/ForecastsRabbitMQProcessor
RUN dotnet build -c Release -o /app

WORKDIR /src/ForecastsCollector
RUN dotnet build -c Release -o /app

WORKDIR /src/ForecastsBlazor
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

WORKDIR /app
COPY --from=build /app .
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "ForecastsBlazor.dll"]