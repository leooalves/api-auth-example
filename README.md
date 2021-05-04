# Authenticaiton and Authorization example

A simple Authentication and Authorization API using .NET 5, JSON Web Token (JWT) and Entity Framework (EF).

## Installing / Getting started

```bash

# Clone this repository
$ git clone https://github.com/leooalves/api-auth-example.git

# Access the project folder cmd/terminal
$ cd api-auth-example

# build the project, installing the dependencies
$ dotnet build

# Run the application in development mode
$ dotnet run

# The server will start at port: 5001 - go to https://localhost:5001

```

You can check the documentation on (https://localhost:5001/swagger).

Also, you can use the endpoint https://localhost:5001/v1 to create the first users. 

By default the API is using InMemoryDatabase but you can change it easily replacing the "AddDbContext" line in Startup.cs.


## Contributing

If you'd like to contribute, please fork the repository and use a feature
branch. Pull requests are warmly welcome.

## License

This repository is licensed with the [MIT](LICENSE) license.
