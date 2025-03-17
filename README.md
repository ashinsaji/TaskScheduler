# Task Scheduler

## Database
The project uses **SQLite** as the database.

## Applying Migrations
To apply migrations, use the following commands:
```sh
 dotnet ef migrations add InitialCreate
 dotnet ef database update
```

## Running Initial SQL Script
Run the initial SQL script located at:
```
~/TaskScheduler/data.sql
```

## Scalar API Reference
The API reference is available at:
```
http://localhost:5272/scalar/v1
```
This endpoint provides documentation for all available APIs.

---

Ensure that you have the required dependencies installed before running the above commands.

