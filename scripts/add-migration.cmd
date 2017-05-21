@echo off
echo/
echo //--------------------------------------------------------------------------
echo // CREATE MIGRATIONS SCRIPT
echo //
echo // Needed input:
echo // ------------
echo // Project name   : The project that contains the DbContext
echo // DbContext name : The DbContext with the required model configuration
echo // Migration name : This script will add the "Migration" suffix to the name
echo //--------------------------------------------------------------------------
echo/
set /p project="Project name   : "
set /p dbContext="DbContext name : "
set /p name="Migration name : "

set scriptsDir=%cd%
set cliProjectDir="..\samples\Demo.cli"

@echo cd %cliProjectDir%
cd %cliProjectDir%

@echo dotnet ef migrations add %name%Migration_%dbContext% -p ..\%project% -c %dbContext%
dotnet ef migrations add %name%Migration_%dbContext% -p ..\%project% -c %dbContext%

cd %scriptsDir%
