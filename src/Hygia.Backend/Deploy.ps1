$ServiceName = "Hygia.Backend"

$service = Get-Service $ServiceName -ErrorAction SilentlyContinue
if (!$service)
{
    Write-Host "The service will be installed"
    & ".\NServiceBus.Host.exe" /install NServiceBus.Production | Write-Host
}