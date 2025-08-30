# ScheduleGenerator

## Описание проекта

Веб-приложение на ASP.NET Core Razor Pages для составления расписания событий и его управлением. 
Для пользователей доступны операции CRUD и предусмотрена валидация вводимых данных.

---

## Первый запуск

1. Скачайте и установите PostgreSQL с официального сайта: https://www.postgresql.org/download/

2. Настройте подключение к базе. В файле `appsettings.json` измените значение Password на свой пароль от сервера БД:
    
    ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=ScheduleDb;Username=postgres;Password=YOUR_PASSWORD"
     }
   }
