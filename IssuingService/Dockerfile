FROM mono
COPY src/IssuingService/bin/Debug /program
WORKDIR /program
USER 1001
EXPOSE 5000
ENTRYPOINT ["mono", "IssuingService.exe"]
