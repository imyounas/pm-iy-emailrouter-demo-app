#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["PM.IY.EmailRouterDemoApp/PM.IY.EmailRouterDemoApp.csproj", "PM.IY.EmailRouterDemoApp/"]
COPY ["ER.Infrastructure/ER.Infrastructure.csproj", "ER.Infrastructure/"]
COPY ["ER.Application/ER.Application.csproj", "ER.Application/"]
COPY ["ER.Domain/ER.Domain.csproj", "ER.Domain/"]
RUN dotnet restore "PM.IY.EmailRouterDemoApp/PM.IY.EmailRouterDemoApp.csproj"
COPY . .
WORKDIR "/src/PM.IY.EmailRouterDemoApp"
RUN dotnet build "PM.IY.EmailRouterDemoApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PM.IY.EmailRouterDemoApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PM.IY.EmailRouterDemoApp.dll"]
