FROM mono
COPY src/RabbitConsumer/bin/Debug /program
WORKDIR /program
ENTRYPOINT ["mono", "RabbitConsumer.exe"]