FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Plinia-AuthService/Plinia-AuthService.csproj", "Plinia-AuthService/"]
RUN dotnet restore "Plinia-AuthService/Plinia-AuthService.csproj"
COPY . .
WORKDIR "/src/Plinia-AuthService"
RUN dotnet build "Plinia-AuthService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Plinia-AuthService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Plinia-AuthService.dll"]
