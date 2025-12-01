New-Alias tn Browse\Test-Host
function Test-Host {
  param(
    [Alias('ComputerName', 'RemoteAddress', 'cn')]
    [string]$HostName,
    [Alias('RemotePort')]
    [string]$Port
  )

  if ($Hostname -match '^\s*\d{1,5}\s*$' -and $Hostname -as [UInt16]) {
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

  $Local:args = $args

  if ($Port) {
    if ($Port -as [UInt16]) {
      $Target.Port = [UInt16]$Port
    }
    else {
      $Local:args = , $Port + $Local:args
    }
  }

  Test-NetConnection @Target @Local:args
}
