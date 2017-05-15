@echo off
set /p project= "Project name...: "
set /p name= "Migration name.: "

@echo cd ..\samples\Demo.cli
cd "..\samples\Demo.cli"

@echo dotnet ef migrations add %name% -p ..\%project%
dotnet ef migrations add %name% -p ..\%project%
