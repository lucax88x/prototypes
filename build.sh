set -e

docker run -v $(pwd)/src:/src mono xbuild /src/HelloWorld/HelloWorld.sln

docker build -t adunicorn/helloworld .
