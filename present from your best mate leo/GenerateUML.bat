@echo off
setlocal

echo PlantUML class diagram generator
echo.

where dotnet >nul 2>&1
if errorlevel 1 (
    echo ERROR: The .NET SDK was not found. Install the .NET SDK, then run this file again.
    pause
    exit /b 1
)

set "PROJECT="
set /p "PROJECT=Enter the full path to your .NET project folder (not the .sln folder, the folder with your .cs files): "

if not defined PROJECT (
    echo ERROR: No project folder was entered.
    pause
    exit /b 1
)

if not exist "%PROJECT%\" (
    echo ERROR: That folder does not exist:
    echo "%PROJECT%"
    pause
    exit /b 1
)

set "PUMLGEN=%USERPROFILE%\.dotnet\tools\puml-gen.exe"

if not exist "%PUMLGEN%" (
    echo Installing PlantUmlClassDiagramGenerator...
    dotnet tool install --global PlantUmlClassDiagramGenerator
    if errorlevel 1 (
        echo.
        echo ERROR: Tool installation failed.
        pause
        exit /b 1
    )
)

if not exist "%PUMLGEN%" (
    echo ERROR: Could not find puml-gen.exe at:
    echo "%PUMLGEN%"
    echo Check that the .NET global tools folder is available, then try again.
    pause
    exit /b 1
)

set "OUTPUT=%PROJECT%\PlantUML_Output"
if not exist "%OUTPUT%" mkdir "%OUTPUT%"

echo.
echo Generating the combined diagram with association lines...
"%PUMLGEN%" "%PROJECT%" "%OUTPUT%" -dir -excludePaths bin,obj,Properties -createAssociation -allInOne

if errorlevel 1 (
    echo.
    echo ERROR: Diagram generation failed.
    pause
    exit /b 1
)

if not exist "%OUTPUT%\include.puml" (
    echo.
    echo ERROR: Generation finished, but include.puml was not found in:
    echo "%OUTPUT%"
    pause
    exit /b 1
)

echo.
echo Diagram generated:
echo "%OUTPUT%\include.puml"
echo.
echo Open editor.plantuml.com in your browser and upload or paste the
echo contents of include.puml into the editor to view the diagram.
pause
