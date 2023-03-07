@echo off
set tagv=v0.0.0.1


docker build -f ".\Dofin360.Admin\Dockerfile" -t git.prima-inform.ru:4567/dofin/dofin360.admin:%tagv% .
docker push git.prima-inform.ru:4567/dofin/dofin360.admin:%tagv%
pause()