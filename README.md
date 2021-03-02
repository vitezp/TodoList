
![Github Actions Pipeline](https://github.com/vitezp/TodoList/actions/workflows/config.yml/badge.svg)


# TodoList 

The project uses .NET Core 5.0 for back-end and simple react.js with typescript in front-end. 

## Getting started

```
$ dotnet build
$ cd src/TodoList.Web/
$ dotnet run
```

Web is served at https://localhost:5001 / http://localhost:5000
Swagger can be found at https://localhost:5001/swagger/index.html

## Repo structure

### src
This repo is structured into 4 main projects 

`TodoList.Application` - Represent the main interface of the system -- it consists of 3rd party tools like mediatR, automapper, fluent validator to make the project clean, structure robust and easy to extend. Only project it references is *TodoList.Domain*.

`TodoList.Domain` - Defines the contract for interaction with the application. SDK client could be easily created referencing this project as it ensures the type consistency. This project does not reference anything from the system. 

`TodoList.Infrastructure` - Contains implementation for the data persistance. Uses Microsoft's SQLite implementation. References the *TodoList.Domain* and implements the interface for storing the items. 

`TodoList.Web` - The main entrypoint of the application. Folder 'client' contains the react frontend application to demonstrate the functionality. It references only *TodoList.Application*. 

`TodoList.Console` - Simple DB client to demonstrate CRUD operations. Used in early stage of development.   

### tests
`TodoList.Application.UnitTests` - Covers validators.
`TodoList.Application.IntegrationTests` - Covers the handlers logic responsible for business logic.
`TodoList.Infrastruction.UnitTests` - Tests the database logic.

## Images

![Frontend](/images/frontend.PNG?raw=true)
![Swagger](/images/swagger.PNG?raw=true)
![Code Coverage](/images/coverage.PNG?raw=true)

## Further continuation
-Dockerize build
-Automated tests for front-end
-Generate SDK client and implement API tests
-Host website in Azure/Heroku...

