@cd %~dp0..\Tool\CfgGen\
@set rootPath=..\..

ConfigGen.exe -optMode all ^
-configXml %rootPath%\Csv\Cfg_Skill.xml ^
-xmlCodeDir %rootPath%\Unity\Assets\Source\Config\Xml



