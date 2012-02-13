$ServiceName = "Hygia.Backend"

$service = Get-Service $ServiceName -ErrorAction SilentlyContinue
if ($service)
{
    Stop-Service $ServiceName -Force
}
