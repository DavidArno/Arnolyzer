echo off
cd bin\Release
erase *.nupkg
C:\Development\NuGet\nuget.exe pack ..\..\Arnolyzer.csproj -Prop Configuration=Release -Symbols
