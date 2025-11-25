New-Alias tn Test-Host
function Test-Host {
  param(
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

  if ($Port) {
    if ($Port -as [uint16]) {
      $Target.Port = [uint16]$Port
    }
    else {
      $args = , $Port + $args
    }
  }

  Test-NetConnection @Target @args
}
