# Redis as a Primary DB using a .NET 6 API

<img src="/pictures/redis.png" title="redis container"  width="900">

## Packages
```
Microsoft.Extensions.Caching.StackExchangeRedis
```

## Docker
```
docker compose up -d
docker compose stop
```

## Redis
```
docker exec -it redis_cache /bin/bash
redis-cli
ping
set key value
get key
del key
exit
```
