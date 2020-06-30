$param = $args[0]

$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
# $apiFolder = Join-Path -Path $dir -ChildPath .\dockapp


if ($param -eq "build") {
    docker-compose.exe -f docker-compose.yml up --build --force-recreate cookbook-admin-ui cookbook-recipe-ui cookbook-comms-api cookbook-recipe-api cookbook-db-mongodb cookbook-redis cookbook-rabbitmq
}
else {
    docker-compose.exe -f docker-compose.yml up  cookbook-admin-ui cookbook-recipe-ui cookbook-comms-api cookbook-recipe-api cookbook-db-mongodb cookbook-redis cookbook-rabbitmq
}