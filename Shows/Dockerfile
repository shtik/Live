FROM microsoft/aspnetcore-build:2 AS build

WORKDIR /src

COPY . .

WORKDIR /src/src/ShtikLive.Shows

RUN dotnet restore

RUN dotnet publish --output /app/ --configuration Release

FROM microsoft/aspnetcore:2

COPY --from=build /app /app/

WORKDIR /app

ENTRYPOINT ["dotnet", "ShtikLive.Shows.dll"]