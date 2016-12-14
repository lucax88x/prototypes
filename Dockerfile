FROM mono
COPY src/HelloWorld/HelloWorld/bin/Debug /program
WORKDIR /program
CMD mono HelloWorld.exe
