FROM mono
COPY src/HelloWorld/IssuingService/bin/Debug /program
WORKDIR /program
ENTRYPOINT ["mono", "IssuingService.exe"]