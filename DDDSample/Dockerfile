FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["DDDSample/DDDSample.csproj", "DDDSample/"]
RUN dotnet restore "DDDSample/DDDSample.csproj"
COPY . .
WORKDIR "/src/DDDSample"
RUN dotnet build "DDDSample.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DDDSample.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DDDSample.dll"]