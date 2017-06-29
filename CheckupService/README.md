Checkup Exec Service uses topshelf.

The following options can be used:

--c configruation flag. If --c doesn't exist, use default configruation
-i timer interval(default: 60000)
-p xml path, including file name(default: D:\CEinfo.xml)
-m maximum entry

Example:

Install Service:
CheckupService.exe --c -i 100000 -p "E:\DiskInfo.xml" -m 50 install

Uninstall Service:
CheckupService.exe uninstall

Start Service:
CheckupService.exe Start

Stop Service:
CheckupService.exe Stop 

Help:
CheckupService.exe Help