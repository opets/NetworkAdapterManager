echo @off

cd Bin

sc stop NetManagerService
sc delete NetManagerService 

sc create NetManagerService binPath= "%~dp0NetManager.Api.exe"
sc failure NetManagerService actions= restart/60000/restart/60000/""/60000 reset= 86400
sc start NetManager.Api.exe

sc config NetManagerService start=auto

cd ..
