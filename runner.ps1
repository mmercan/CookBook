$param = $args[0]

$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
# $apiFolder = Join-Path -Path $dir -ChildPath .\dockapp


if ($param -eq "build") {
    docker-compose.exe -f docker-compose.yml up --build --force-recreate cookbook-recipe-api cookbook-comms-api cookbook-comms-handler cookbook-recipe-handler #cookbook-admin-ui cookbook-recipe-ui # cookbook-db-mongodb  cookbook-rabbitmq cookbook-redis 
}
else {
    docker-compose.exe -f docker-compose.yml up  cookbook-admin-ui cookbook-recipe-ui cookbook-comms-api cookbook-recipe-api cookbook-db-mongodb cookbook-redis cookbook-rabbitmq
}