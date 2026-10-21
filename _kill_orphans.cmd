@echo off
REM Mata cualquier proceso dotnet.exe que NO sea el actual (la consola donde corre este script).
REM Útil antes de hacer "dotnet run" cuando quedan instancias zombie de runs anteriores
REM que dejan puertos ocupados y rompen el siguiente arranque (ERR_CONNECTION_REFUSED).
echo Matando dotnet.exe huerfanos...
for /f "tokens=2" %%P in ('tasklist /FI "IMAGENAME eq dotnet.exe" ^| findstr /B "dotnet.exe"') do (
    echo   Terminando PID %%P
    taskkill /F /PID %%P >NUL 2>&1
)
echo Listo.