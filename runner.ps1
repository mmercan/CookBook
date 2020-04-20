$param = $args[0]

$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
# $apiFolder = Join-Path -Path $dir -ChildPath .\dockapp


if ($param -eq "build") {
    docker-compose.exe -f docker-compose.yml up --build --force-recreate cookbook-db-mongodb comms-api
}
else {
    docker-compose.exe -f docker-compose.yml up cookbook-db-mongodb comms-api
}