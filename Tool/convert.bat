@echo off & 大小转换工具
cd /d %~dp0

pause
exit
setlocal enabledelayedexpansion
 
for /f "delims=" %%a in ('dir /s/b') do (
    set "RelativePath=%%~a"
    set "RelativePath=!RelativePath:%~dp0=!
    call :Convert
)
pause
exit
 
 
:Convert
if not defined RelativePath goto :eof
if "!RelativePath:~-1!"=="\" set "RelativePath=!RelativePath:~,-1!"
 
for %%i in ("!RelativePath!") do (
    set "Name=%%~nxi"
    for %%j in (a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z) do (
        set "Name=!Name:%%~j=%%~j!"
    )
    ren "!RelativePath!" "!Name!"
    set "RelativePath=%%~dpi"
    set "RelativePath=!RelativePath:%~dp0=!
)
goto Convert