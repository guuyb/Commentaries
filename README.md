# Подготовка инфраструктуры
    > docker run -d --name rabbitmq-with-management -p 5672:5672 -p 15672:15672 rabbitmq:3-management
    > docker run -d --name postgres -p 5432:5432 -e POSTGRES_PASSWORD=changeme postgres:13.3

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

# Тесты
Представлены проектами:
- Commentaries.Data.Test
- Commentaries.Domain.Test

## Commentaries.Data.Test
Тесты выполняются на данных БД из подключения.

## Commentaries.Domain.Test
Тесты выполняются с использованием in-memory БД.

## Commentaries.Client.Test
Тесты выполняются с использованием запущенного `Commentaries.Api`.

## Запуск тестов из cmd
    > dotnet test Commentaries.sln

или

    > dotnet test Commentaries.Data.Test.dll
    > dotnet test Commentaries.Domain.Test.dll
