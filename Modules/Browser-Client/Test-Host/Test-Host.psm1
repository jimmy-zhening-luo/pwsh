New-Alias tn Test-Host
function Test-Host {
  [OutputType([Object])]
  param(
    [Parameter(Mandatory)]
    [Alias('ComputerName', 'RemoteAddress', 'cn')]
    [string]$HostName,
    [Alias('RemotePort', 'p')]
    [string]$Port
  )

  if ($Hostname -match '^\s*\d{1,5}\s*$' -and $Hostname -as [uint16]) {
    if ($Port) {
      $Hostname, $Port = $Port, $Hostname
    }
    else {
      throw 'No hostname provided'
    }
  }

  $Target = @{
    ComputerName = $HostName
  }
  $Argument = ''

  if ($Port) {
    if ($Port -as [uint16]) {
      $Target.Port = [uint16]$Port
    }
    else {
      $Argument = $Port
    }
  }

  if ($Argument) {
    Test-NetConnection @Target $Argument @args
  }
  else {
    Test-NetConnection @Target @args
  }
}
