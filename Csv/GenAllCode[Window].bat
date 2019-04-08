@cd %~dp0..\Tool\CfgGen\
@set rootPath=..\..

@REM 各个平台的资源路径均小写
ConfigGen.exe -optMode all ^
-configXml %rootPath%\Csv\Cfg.xml ^
-luaDir %rootPath%\Code\Scripts\Cfg ^
-dataDir %rootPath%\GamePlayer\config\csv ^
-codeDir %rootPath%\Unity\Assets\Source\Config\Csv



