New-Alias tn Test-Host
function Test-Host {
  [OutputType([Object])]
  param(
    [Parameter(
      Mandatory,
      Position = 0
    )]
    [Alias("ComputerName", "RemoteAddress", "cn")]
    [Parameter(Position = 1)]
    [string]$HostName,
    [Parameter(Position = 2)]
    [Alias("RemotePort", "p")]
    [uint16]$Port
  )

  $Target = @{
    ComputerName = $HostName
  }

  if ($Port) {
    $Target.Port = $Port
  }

  Test-NetConnection @Target
}
