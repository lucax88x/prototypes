docker network create  --driver=overlay mynet

docker service create --name=web      --replicas=1 --network=mynet --publish 9000:9000                       adunicorn/issuing
docker service create --name=rabbit   --replicas=1 --network=mynet --publish 5672:5672 --publish 15672:15672 rabbitmq:3-management
docker service create --name=redis    --replicas=1 --network=mynet --publish=6379:6379                       redis
docker service create --name=consumer --replicas=1 --network=mynet                                           adunicorn/rabbitconsumer
