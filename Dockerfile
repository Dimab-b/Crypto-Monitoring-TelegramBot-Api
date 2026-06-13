FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ApiWithOtherApi.csproj", "./"]
RUN dotnet restore "ApiWithOtherApi.csproj"

COPY . .
RUN dotnet publish "ApiWithOtherApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .


EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ApiWithOtherApi.dll"]