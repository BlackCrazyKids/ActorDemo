@cd %~dp0..\Tool\CfgGen\
@set rootPath=..\..

ConfigGen.exe -optMode all ^
-configXml %rootPath%\Csv\Cfg.xml ^
-dataDir %rootPath%\GamePlayer\Config\Csv ^
