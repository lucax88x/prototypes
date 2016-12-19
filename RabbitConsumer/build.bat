docker run -v /work/prototypes/RabbitConsumer/src:/src mono nuget restore /src/RabbitConsumer.sln
docker run -v /work/prototypes/RabbitConsumer/src:/src mono xbuild /src/RabbitConsumer.sln

docker build -t adunicorn/rabbitconsumer .