echo @off

cd ..

RMDIR /S /Q _Publish\Bin

dotnet publish --configuration Release --self-contained -r win10-x64 

MOVE /Y NetManager.Api\bin\Release\netcoreapp2.1\win10-x64\publish _Publish\Bin 

cd _Publish
