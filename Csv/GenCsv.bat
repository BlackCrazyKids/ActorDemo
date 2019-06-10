@cd %~dp0../Tool/CfgGen/
@set rootPath=../..

ConfigGen.exe -configXml %rootPath%\Csv\Cfg.xml ^
-data %rootPath%\GamePlayer\Data\config\csv ^
-lua %rootPath%\Code\Scripts\Cfg

pause