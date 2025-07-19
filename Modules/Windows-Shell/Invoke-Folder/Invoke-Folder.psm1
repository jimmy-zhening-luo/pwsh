New-Alias -Name explore -Value Invoke-Folder -Option ReadOnly
New-Alias -Name e -Value Invoke-Folder -Option ReadOnly
function Invoke-Folder {
  param(
    [Parameter(ValueFromPipeline)]
    [string]$Path = "."
  )

  process {
    if (Test-Path $Path) {
      if ((Get-Item $Path).PSIsContainer) {
        Invoke-Item -Path $Path @args
      }
      else {
        Edit-File -Path $Path @args
      }
    }
    else {
      throw "Folder '$Path' does not exist."
    }
  }
}

New-Alias -Name e. -Value Invoke-Parent -Option ReadOnly
New-Alias -Name e.. -Value Invoke-Parent -Option ReadOnly
function Invoke-Parent {
  Invoke-Item -Path ($PWD | Split-Path)
}

New-Alias -Name e~ -Value Invoke-Home -Option ReadOnly
function Invoke-Home {
  Invoke-Item $HOME
}

New-Alias -Name e\ -Value Invoke-Drive -Option ReadOnly
New-Alias -Name e/ -Value Invoke-Drive -Option ReadOnly
function Invoke-Drive {
  Invoke-Item $PWD.Drive.Root
}
