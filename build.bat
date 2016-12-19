docker run -v $(pwd)/src:/src mono nuget restore /src/Issuing.sln
docker run -v /work/prototypes/src:/src mono xbuild /src/Issuing.sln

docker build -t adunicorn/issuing .
