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
    [System.UInt16]$Port
  )

  if ($Port) {
    Test-NetConnection -ComputerName $HostName -Port $Port
  }
  else {
    Test-NetConnection -ComputerName $HostName
  }
}
