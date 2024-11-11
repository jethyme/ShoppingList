# Используем образ SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем все проекты в контекст сборки
COPY ["DBData/DBData.csproj", "DBData/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["ShoppingList/ShoppingList.csproj", "ShoppingList/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["ConsoleUI/ConsoleUI.csproj", "ConsoleUI/"]

# Восстанавливаем зависимости
RUN dotnet restore "DBData/DBData.csproj"

# Копируем все остальные файлы
COPY . .

# Собираем проект
RUN dotnet build "DBData/DBData.csproj" -c Release -o /app/build

# Публикация приложения
FROM build AS publish
RUN dotnet publish "DBData/DBData.csproj" -c Release -o /app/publish

# Создаем финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DBData.dll"]
