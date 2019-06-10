@cd %~dp0..\Tool\CfgGen\
@set rootPath=..\..

@REM 各个平台的资源路径均小写
ConfigGen.exe -configXml %rootPath%\Csv\Cfg.xml ^
-lua %rootPath%\Code\Scripts\Cfg ^
-data %rootPath%\GamePlayer\Data\config\csv ^
-csharp %rootPath%\Unity\Assets\Source\Config\CSharp

pause

