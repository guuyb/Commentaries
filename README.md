# Подготовка инфраструктуры
    > docker run -d --name rabbitmq-with-management -p 5672:5672 -p 15672:15672 rabbitmq:3-management
    > docker run -d --name postgres -p 5432:5432 -e POSTGRES_PASSWORD=changeme postgres:13.3

# Необходимые проекты в зависимостях
Следует подтянуть исходники из репозитория [Guuyb.Mq](https://github.com/guuyb/Guuyb.Mq)
- Guuyb.Mq
Следует подтянуть исходники из репозитория [Guuyb.OutboxMessaging](https://github.com/guuyb/Guuyb.OutboxMessaging)
- Guuyb.OutboxMessaging.Data
- Guuyb.OutboxMessaging.Worker

# Commentaries.Api
Предоставляет функционал для создания обсуждений по сущности (есть определенный ИД).
Возможности:
- добавить черновой комментарий к сущности
- опубликовать новый комментарий по сущности
- отредактировать содержание комментария
- опубликовать комментарий
- получить список комментариев по сущности
- добавить файл к комментарию
- получить файл (контент и метаданные)
- удалить файл из комментария

# Миграции для PostgreSql
    > cd Commentaries.Infrastructure
    > dotnet ef migrations add InitialCreate --output-dir ./SecondaryAdapters/Db/Migrations

# Тесты
Представлены проектами:
- Commentaries.Application.Test
- Commentaries.Client.Test
- Commentaries.Infrastructure.Test

## Commentaries.Data.Test
Тесты выполняются на данных БД из подключения.

## Commentaries.Domain.Test
Тесты выполняются с использованием in-memory БД.

## Commentaries.Client.Test
Тесты выполняются с использованием WebApplicationFactory.

## Запуск тестов из cmd
    > dotnet test Commentaries.sln

или

    > dotnet test Commentaries.Application.Test
    > dotnet test Commentaries.Client.Test
    > dotnet test Commentaries.Infrastructure.Test
