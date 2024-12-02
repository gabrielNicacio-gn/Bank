
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY src/Bank /App/src
COPY src/Bank/Bank.csproj /App
RUN dotnet restore      

WORKDIR /App
RUN dotnet publish -c Release -o bank

FROM mcr.microsoft.com/dotnet/aspnet:8.0

COPY --from=build-env /App/bank .
EXPOSE 8080
#ENTRYPOINT ["dotnet", "Bank.dll"]
CMD ["dotnet","run","Bank.dll"]