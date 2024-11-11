FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["DBData/DBData.csproj", "DBData/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["ShoppingList/ShoppingList.csproj", "ShoppingList/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["ConsoleUI/ConsoleUI.csproj", "ConsoleUI/"]

RUN dotnet restore "DBData/DBData.csproj"

COPY . .

RUN dotnet build "DBData/DBData.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DBData/DBData.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DBData.dll"]
