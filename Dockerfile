FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

WORKDIR /src

COPY ["Matvi.Front.UI/Matvi.Front.UI.csproj", "Matvi.Front.UI/"]
COPY ["Matvi.Back.Application/Matvi.Back.Application.csproj", "Matvi.Back.Application/"]
COPY ["Matvi.Back.Infrastructure/Matvi.Back.Infrastructure.csproj", "Matvi.Back.Infrastructure/"]
COPY ["Matvi.Transversal/Matvi.Transversal.csproj", "Matvi.Transversal/"]

RUN dotnet restore "Matvi.Front.UI/Matvi.Front.UI.csproj"

COPY . .

WORKDIR "/src/Matvi.Front.UI"

RUN dotnet publish "Matvi.Front.UI.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine-extra AS final

RUN apk add --no-cache tzdata

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 7022

ENTRYPOINT ["dotnet", "Matvi.Front.UI.dll"]