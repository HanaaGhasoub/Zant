#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PhoneNumberTopUp.Data/PhoneNumberTopUp.Data.csproj", "PhoneNumberTopUp.Data/"]
COPY ["PhoneNumberTopUp.Domain/PhoneNumberTopUp.Domain.csproj", "PhoneNumberTopUp.Domain/"]
COPY ["PhoneNumberTopUp.API/PhoneNumberTopUp.API.csproj", "PhoneNumberTopUp.API/"]
RUN dotnet restore "./PhoneNumberTopUp.Data/PhoneNumberTopUp.Data.csproj"
RUN dotnet restore "./PhoneNumberTopUp.Domain/PhoneNumberTopUp.Domain.csproj"
RUN dotnet restore "./PhoneNumberTopUp.API/PhoneNumberTopUp.API.csproj"
COPY . .
WORKDIR "/src/PhoneNumberTopUp.API"
RUN dotnet build "./PhoneNumberTopUp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PhoneNumberTopUp.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhoneNumberTopUp.API.dll"]