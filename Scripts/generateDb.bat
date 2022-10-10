mkdir ..\NewConf
copy .\template.xml ..\NewConf\settings.xml
sqlite3 ..\NewConf\ImageDb.db ".read init.sql"
