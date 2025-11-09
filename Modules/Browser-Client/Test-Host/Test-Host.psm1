New-Alias tn Test-Host
<#
.FORWARDHELPTARGETNAME Test-NetConnection
.FORWARDHELPCATEGORY Function
#>
function Test-Host {
  [OutputType([Object])]
  param(
    [Parameter(Mandatory)]
    [Alias("ComputerName", "RemoteAddress", "cn")]
    [string]$HostName,
    [Alias("RemotePort", "p")]
    [uint16]$Port
  )

  if ($Port) {
    Test-NetConnection -ComputerName $HostName -Port $Port
  }
  else {
    Test-NetConnection -ComputerName $HostName
  }
}
