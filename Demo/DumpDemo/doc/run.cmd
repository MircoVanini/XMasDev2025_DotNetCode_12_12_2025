rem Install ProcDump as the AeDebug postmortem debugger (https://learn.microsoft.com/en-us/windows/win32/com/aedebug)
rem https://learn.microsoft.com/en-us/sysinternals/downloads/procdump
rem https://learn.microsoft.com/en-us/windows/win32/debug/configuring-automatic-debugging#configuring-automatic-debugging-for-application-crashes
rem 
rem
c:\tools\SysinternalsSuite\procdump -ma -i C:\dumps

rem Uninstall ProcDump as the AeDebug postmortem debugger
rem 
rem c:\tools\SysinternalsSuite\procdump -u