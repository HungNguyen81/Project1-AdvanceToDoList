@echo off

:: BatchGotAdmin
:-------------------------------------
REM  --> Check for permissions
    IF "%PROCESSOR_ARCHITECTURE%" EQU "amd64" (
>nul 2>&1 "%SYSTEMROOT%\SysWOW64\cacls.exe" "%SYSTEMROOT%\SysWOW64\config\system"
) ELSE (
>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"
)

REM --> If error flag set, we do not have admin.
if '%errorlevel%' NEQ '0' (
    echo Requesting administrative privileges...
    goto UACPrompt
) else ( goto gotAdmin )

:UACPrompt
    echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\getadmin.vbs"
    set params= %*
    echo UAC.ShellExecute "cmd.exe", "/c ""%~s0"" %params:"=""%", "", "runas", 1 >> "%temp%\getadmin.vbs"

    "%temp%\getadmin.vbs"
    del "%temp%\getadmin.vbs"
    exit /B

:gotAdmin
    pushd "%CD%"
    CD /D "%~dp0"
:-------------------------------------- 
@echo off
color 02
set dest1=%USERPROFILE%\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup
set dest2=%AllUsersProfile%\desktop
echo %~d0
set src=%cd%
echo %dir%
cscript preSetup.vbs "%dest1%\AdvancedTDL.lnk" "%src%\AdvanceTDL.exe"
cscript preSetup.vbs "%dest2%\AdvancedTDL.lnk" "%src%\AdvanceTDL.exe"
echo SUCCESSFULLY SETUP ADVANCED TO DO LIST APP
pause
start "" AdvanceTDL.exe
exit
pause