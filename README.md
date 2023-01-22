# Book Story

## Технологии

- [.NET 5.0.4](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
- [ASP.NET Core 5](https://docs.microsoft.com/en-us/aspnet/core)
- [Entity Framework Core 5.0.17](https://learn.microsoft.com/en-us/ef/core/)
- [PostgreSQL 14](https://www.postgresql.org/)

## Дополнительные инструменты

- [Swagger UI](https://swagger.io/)
- [Automapper](https://automapper.org/)

## Запуск приложения

### PostgreSQL режим
0. Убедитесь в том, что в `./BookStory/appsettings.Deployment.json` `InMemory=false`.
1. Убедитесь в том, что у вас развернут инстанс [PostgreSQL Server](https://www.postgresql.org/download/),
2. Указать пользователя БД для выполнения операций миграций в `./BookStory/appsettings.Deployment.json`, по-умолчанию указан `postgres`.
3. Выполнить миграции вручную (если в конфиге `MigrateAuto=false`)
```
cd ./BookStory/
dotnet ef database update --project ../BookStory.Data/
```
4. Либо, миграции выполнятся автоматически, если `MigrateAuto=true`.
5. Выполнить сборку проекта (из директории `./BookStory/`) `dotnet build`
6. Запустить проект в режиме `Kestrel`: `dotnet run -c Debug --launch-profile "BookStory"`
7. Приложение будет развернуто `http://localhost:5018`, для использования Swagger'a, необходимо перейти к `http://localhost:5018/swagger/index.html`.

### InMemory режим
1. Убедитесь в том, что в `./BookStory/appsettings.Deployment.json` `InMemory=true`.
2. Выполнить сборку проекта (из директории `./BookStory/`) `dotnet build`
3. Запустить проект в режиме `Kestrel`: `dotnet run -c Debug --launch-profile "BookStory"`
4. Приложение будет развернуто `http://localhost:5018`, для использования Swagger'a, необходимо перейти к `http://localhost:5018/swagger/index.html`.

## Тестирование `endpoints`

1. Можно воспользоваться готовым набором запросов для Postman'a: `postman_collection_v2.X.json`.
