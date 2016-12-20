ECHO Creating machine node1
docker-machine create node1 -d virtualbox
docker-machine stop node1
VBoxManage.exe sharedfolder add node1 --name "/work" --hostpath "\\?\C:\Work\Git\" --automount
docker-machine start node1
docker-machine regenerate-certs -f node1

ECHO Creating the swarm
FOR /f "tokens=*" %%i IN ('docker-machine env node1') DO @%%i
FOR /f "delims=" %%a in ('docker-machine ip node1') do @set ip1=%%a
docker swarm init --advertise-addr=%ip1%
FOR /f "delims=" %%a in ('docker swarm join-token -q worker') do @set swarmToken=%%a
ECHO %swarmToken%
