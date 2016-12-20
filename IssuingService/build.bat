@echo off

docker run -v /work/prototypes/IssuingService/src:/src mono nuget restore /src/Issuing.sln
docker run -v /work/prototypes/IssuingService/src:/src mono xbuild /src/Issuing.sln

docker build -t adunicorn/issuing .
