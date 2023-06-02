#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Store.Mvc/Store.Mvc.csproj", "Store.Mvc/"]
COPY ["Store.Application/Store.Application.csproj", "Store.Application/"]
COPY ["Store.Domain/Store.Domain.csproj", "Store.Domain/"]
RUN dotnet restore "Store.Mvc/Store.Mvc.csproj"
COPY . .
WORKDIR "/src/Store.Mvc"
RUN dotnet build "Store.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Store.Mvc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Store.Mvc.dll"]