New-Alias tn Test-Host
function Test-Host {
  [OutputType([Object])]
  param(
    [Parameter(Mandatory)]
    [Alias('ComputerName', 'RemoteAddress', 'cn')]
    [string]$HostName,
    [Alias('RemotePort', 'p')]
    [string]$Port,
    [Parameter(ValueFromRemainingArguments)]
    [string[]]$Arguments
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
      $Arguments = , $Port + $Arguments
    }
  }

  Test-NetConnection @Target @Arguments
}
