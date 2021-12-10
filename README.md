# DotNetCore-Couchbase-TodoApp-WebApi

# Prerequests

- Project already have a docker-compose file for dockerize Asp.Net Core WebApi and Couchbase Server.

- You can run docker-compose file by Package Manager Console into Visual Studio Envirenment or Command Line on your Windows Machine.

- Simply you can type and run commands following:

  ```
  docker-compose build
  ```

  ```
  docker-compose up -d
  ```

## Couchbase Server Configuration

- After installation you should configure your Couchbase Server. 

- You can achieve Couchbase Server GUI by following instructions:

- On your browser, go to url

  ```
  http://localhost:8091/ui/index.html
  ```

 ### Setting up  required fields on configuration screen following:

- Username: Administrator

- Password: 123456

### Add a Bucket

- Bucket Name: TodoApp

Finally, you need to create an index for complete couchbase configuration

- Go to Query Editor clicking by Query tab and type query following:

  ```
  CREATE INDEX ix_type ON TodoApp(type)
  ```

-  Click the Execute button for run query

- if you have a result after execution following, its sounds like configurations completed successfully 

  ```
  {
    "results": []
  }
  ```

# API Explanation

There are two way posting requests to our Restful Api. You can you Swagger UI or Postman

If you want to use swagger you should go to following url:

  ```
  http://localhost:5000/swagger/index.html
  ```

- First of all, you need to create an api user. You can use Register tab and post your informations.
- After that you have to login and get an Access Token. So, you can copy and paste this token in the Authorize section without Bearer keyword.
- After authentication process, you can try crud users, categories and tasks.

# Used Technologies and Packages Stack

- Asp.Net Core 5 (.Net 5)  (Used for WebAPI and UnitTests Layers)
- .Net Standart 2.1 (Used for Business, DataAccess, Entities and Core layers)
- CouchbaseNetClient
- Linq2Couchbase
- AutoMapper
- Fluent Validation
- Fluent Assertions
- Autofac
- Autofac.Extensions.DependencyInjection
- Autofac.Extras.DynamicProxy
- Castle.Core
- log4net
- NUnit
- Moq
- JWT
- Swashbuckle.AspNetCore (Used for Swagger UI)
- Docker
- Docker Compose

# Unit Tests

- WhenCorrectPasswordForExistingEmailIsGiven_Login_ReturnsUserForCorrectEmail

- WhenValidInputAreGiven_Register_ShouldBeReturnAdded

- WhenExistUserEmailIsGiven_Register_ShouldBeReturnUserAlreadyExists
