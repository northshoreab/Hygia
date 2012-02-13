$ServiceName = "Hygia.Backend"

$service = Get-Service $ServiceName -ErrorAction SilentlyContinue
if (!$service)
{
    Start-Service $ServiceName -Force
}
