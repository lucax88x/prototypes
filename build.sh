set -e

docker run -v $(pwd)/src:/src mono xbuild /src/Issuing.sln

docker build -t adunicorn/issuing .
