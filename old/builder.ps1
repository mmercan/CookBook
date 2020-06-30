$param = $args[0]

$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
# $apiFolder = Join-Path -Path $dir -ChildPath .\dockapp

# az acr login --name matttestacr01
# docker login matttestacr01.azurecr.io -u matttestacr01 -p {{Password}}

docker-compose.exe -f Comms.Api/docker-compose.yml build

docker-compose.exe -f Comms.Api/docker-compose.yml push