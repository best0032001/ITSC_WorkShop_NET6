FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY [".", "."]
RUN dotnet restore "Demo.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "Demo.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Demo.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
RUN apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk update && \
    apk add --no-cache tzdata
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Demo.dll"]

