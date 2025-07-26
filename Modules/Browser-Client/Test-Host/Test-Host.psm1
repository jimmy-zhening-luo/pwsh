New-Alias tn Test-Host
<#
.FORWARDHELPTARGETNAME Test-NetConnection
#>
function Test-Host {
  param(
    [Parameter(Mandatory)]
    [Alias("ComputerName", "RemoteAddress", "cn")]
    [System.String]$HostName,
    [Alias("RemotePort", "p")]
    [System.Int32]$Port
  )
  $Splat = @{
    ComputerName = $HostName
  }

  if ($Port) {
    $Splat.Add("Port", $Port)
  }

  Test-NetConnection @Splat
}
