Checkup Exec Service uses topshelf.

The following options can be used:

-i timer interval(default: 60000)
-p xml path, including file name(default: D:\CEinfo.xml)


Example:

Install Service:
CheckupService.exe -i 100000 -p "E:\DiskInfo.xml" install

Uninstall Service:
CheckupService.exe uninstall

Start Service:
CheckupService.exe Start

Stop Service:
CheckupService.exe Stop 

Help:
CheckupService.exe Help