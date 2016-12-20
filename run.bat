docker service create --name=web2 --replicas=5 --publish 9001:9000 adunicorn/issuing
docker service ps web2
REM docker-compose up