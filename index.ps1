$Env:PSModulePath += "$PSScriptRoot\Module;"
$Global:InformationPreference = 'Continue'
$Global:PSDefaultParameterValues = @{
  'Clear-RecycleBin:Force'   = $True
  'Format-Table:Wrap'        = $True
  'Get-AppxPackage:AllUsers' = $True
  'Get-Process:ErrorAction'  = 'SilentlyContinue'
  'Get-WindowsDriver:All'    = $True
  'Get-WindowsDriver:Online' = $True
  'Install-Module:Force'     = $True
  'Install-Module:Scope'     = 'AllUsers'
  'Invoke-WebRequest:Method' = 'GET'
  'Remove-Item:Force'        = $True
  'Stop-Service:Force'       = $True
  'Update-Help:Scope'        = 'AllUsers'
}

@(
  'clear'
  'rd'
  'man'
  'gm'
  'gp'
  'gu'
) | Remove-Alias -Force

& {
  $DIST = "$PSScriptRoot\Class\bin\Release\net9.0-windows\Module.dll"
  $MODULE = "$PSScriptRoot\Module\Module"
  $ASSEMBLY = "$MODULE\Module.dll"

  if (Test-Path -LiteralPath $DIST -PathType Leaf) {
    if (
      -not (
        Test-Path -LiteralPath $ASSEMBLY -PathType Leaf
      ) -or (
        (
          Get-ItemPropertyValue -LiteralPath $ASSEMBLY -Name LastWriteTime
        ) -ne (
          Get-ItemPropertyValue -LiteralPath $DIST -Name LastWriteTime
        ) -and (
          Get-FileHash -LiteralPath $ASSEMBLY -Algorithm MD5
        ).Hash -ne (
          Get-FileHash -LiteralPath $DIST -Algorithm MD5
        ).Hash
      )
    ) {
      Get-Process -Name pwsh -ErrorAction SilentlyContinue |
        Where-Object -Property Id -NE -Value $PID |
        ForEach-Object -MemberName Kill -ArgumentList $true

      Copy-Item -LiteralPath $DIST -Destination $MODULE -Force -ErrorAction Continue
    }
  }
  else {
    Write-Warning -Message 'Module assembly is not built.'
  }
}

if ($null -ne $Env:SSH_CLIENT) {
  & {
    Import-Module PSReadLine

    @(
      @{
        Chord    = 'Shift+DownArrow'
        Function = 'NextHistory'
      }
      @{
        Chord    = 'Shift+UpArrow'
        Function = 'PreviousHistory'
      }
      @{
        Chord    = 'Ctrl+RightArrow'
        Function = 'ForwardWord'
      }
      @{
        Chord    = 'Shift+Ctrl+RightArrow'
        Function = 'SelectForwardWord'
      }
      @{
        Chord    = 'Ctrl+d'
        Function = 'SwitchPredictionView'
      }
      @{
        Chord    = 'Ctrl+D'
        Function = 'SwitchPredictionView'
      }
    ) |
      ForEach-Object -Process {
        Set-PSReadLineKeyHandler @PSItem
      }
  }

  if ($PWD.Path -eq $HOME) {
    Set-Location -LiteralPath code
  }
}
