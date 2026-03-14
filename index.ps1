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
  'rd'
  'gm'
  'gp'
  'gu'
) | Remove-Alias -Force

& {
  $MODULE = 'PowerModule'
  $SOURCE = "$PSScriptRoot\Class"
  $BUILD = "$PSScriptRoot\Build\bin\release\$MODULE.dll"
  $INSTALL = "$HOME\Documents\PowerShell\Modules\$MODULE"

  if (Test-Path -LiteralPath $BUILD -PathType Leaf) {
    if (
      -not (
        Test-Path -LiteralPath $INSTALL\$MODULE.dll -PathType Leaf
      ) -or (
        (
          Get-ItemPropertyValue -LiteralPath $INSTALL\$MODULE.dll -Name LastWriteTime
        ) -ne (
          Get-ItemPropertyValue -LiteralPath $BUILD -Name LastWriteTime
        ) -and (
          Get-FileHash -LiteralPath $INSTALL\$MODULE.dll -Algorithm MD5
        ).Hash -ne (
          Get-FileHash -LiteralPath $BUILD -Algorithm MD5
        ).Hash
      )
    ) {
      Get-Process -Name pwsh -ErrorAction SilentlyContinue |
        Where-Object -Property Id -NE -Value $PID |
        ForEach-Object -MemberName Kill -ArgumentList $true

      if (
        -not (
          Test-Path -LiteralPath $INSTALL -PathType Container
        )
      ) {
        New-Item -LiteralPath $INSTALL -ItemType Directory -ErrorAction Continue
      }

      Copy-Item -LiteralPath $BUILD -Destination $INSTALL -Force -ErrorAction Continue
      Copy-Item -LiteralPath $SOURCE\$MODULE.psd1 -Destination $INSTALL -Force -ErrorAction Continue
    }
  }
  else {
    Write-Warning -Message 'PowerModule assembly is not built.'
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
