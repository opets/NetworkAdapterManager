echo @off

cd Bin

sc stop NetManagerService
sc delete NetManagerService 

sc create NetManagerService binPath= "%~dp0Bin\NetManager.Api.exe"
sc description NetManagerService "Network Adapter Manager Web Service"
sc failure NetManagerService actions= restart/60000/restart/60000/""/60000 reset= 86400
sc start NetManagerService

sc config NetManagerService start=auto

cd ..
