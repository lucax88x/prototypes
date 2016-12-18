FROM mono
COPY src/IssuingService/bin/Debug /program
WORKDIR /program
ENTRYPOINT ["mono", "IssuingService.exe"]