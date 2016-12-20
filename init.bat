ECHO Creating machine issuingcompose
docker-machine create issuingcompose -d virtualbox
docker-machine stop issuingcompose
VBoxManage.exe sharedfolder add issuingcompose --name "/work" --hostpath "\\?\C:\Work\Git\" --automount
docker-machine start issuingcompose
docker-machine regenerate-certs -f issuingcompose

ECHO Creating the swarm
FOR /f "tokens=*" %%i IN ('docker-machine env issuingcompose') DO @%%i
FOR /f "delims=" %%a in ('docker-machine ip issuingcompose') do @set ip1=%%a
docker swarm init --advertise-addr=%ip1%
FOR /f "delims=" %%a in ('docker swarm join-token -q worker') do @set swarmToken=%%a
ECHO swarmToken