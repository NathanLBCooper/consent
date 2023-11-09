# consent

...

# Tools

## Lefthook

Pre-commit hooks

```sh
winget install evilmartians.lefthook
```

then to sync githooks with *lefthook.yml*

```sh
lefthook install
```

# Setup

```sh
docker-compose up --build
```

This will start and migrate the sql-server container, as well as start the asp.net api.

Browse to http://localhost:33080/swagger/index.html for api documentation.

# Test #

Manually make sure the docker SQL sql-server is running, then run the C# tests.

