@echo off

cd RabbitConsumer
build.bat
cd ..

cd IssuingService
build.bat
cd ..

echo Done
