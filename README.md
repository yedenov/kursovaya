# Система учета посещаемости студентов

Веб-приложение для учета посещаемости студентов на занятиях, разработанное с использованием ASP.NET Core MVC.

## Функциональность

- Управление списком студентов
- Управление списком занятий
- Учет посещаемости студентов
- Фильтрация и сортировка данных
- Пагинация списков

## Технологии

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Bootstrap
- jQuery

## Требования

- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 или JetBrains Rider

## Установка и запуск

1. Клонируйте репозиторий:
```bash
<<<<<<< HEAD
git clone [url-репозитория]
=======
git clone https://github.com/yedenov/kursovaya.git
>>>>>>> a6467fcbdd7fb08a2ca2aee23e0cc5c937087937
```

2. Откройте решение в Visual Studio или Rider

3. Обновите строку подключения к базе данных в `appsettings.json`:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=aspnetcore;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

4. Примените миграции базы данных:
```bash
dotnet ef database update
```

5. Запустите приложение:
```bash
dotnet run
```

## Структура проекта

- `Controllers/` - контроллеры MVC
- `Models/` - модели данных
- `Views/` - представления Razor
- `Data/` - контекст базы данных и миграции
- `wwwroot/` - статические файлы (CSS, JavaScript)

## Валидация данных

- Клиентская валидация с использованием jQuery Validation
- Серверная валидация с использованием Data Annotations
- Проверка обязательных полей
- Проверка корректности данных

## Развертывание

1. Опубликуйте приложение:
```bash
dotnet publish -c Release
```

2. Настройте веб-сервер (IIS, Nginx, Apache)

3. Настройте строку подключения к базе данных

4. Примените миграции на сервере

## Лицензия

MIT 
