# Система учета посещаемости

Веб-приложение для учета посещаемости студентов на занятиях, разработанное на ASP.NET Core.

## Функциональность

- Управление списком студентов
- Управление списком занятий
- Учет посещаемости студентов
- Просмотр статистики посещаемости

## Технологии

- ASP.NET Core
- Entity Framework Core
- SQL Server
- Bootstrap 5
- jQuery

## Требования

- .NET 7.0 или выше
- SQL Server
- Visual Studio 2022 или Visual Studio Code

## Установка

1. Клонируйте репозиторий:
```bash
git clone https://github.com/your-username/attendance-system.git
```

2. Откройте решение в Visual Studio или Visual Studio Code

3. Обновите строку подключения к базе данных в `appsettings.json`

4. Выполните миграции базы данных:
```bash
dotnet ef database update
```

5. Запустите приложение:
```bash
dotnet run
```

## Структура проекта

- `Controllers/` - контроллеры приложения
- `Models/` - модели данных
- `Views/` - представления
- `Data/` - контекст базы данных и миграции
- `wwwroot/` - статические файлы (CSS, JavaScript, изображения)

## Лицензия

MIT 