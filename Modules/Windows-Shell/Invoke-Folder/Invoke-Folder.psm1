New-Alias -Option ReadOnly -Name explore -Value Invoke-Folder
New-Alias -Option ReadOnly -Name e -Value Invoke-Folder
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

New-Alias -Option ReadOnly -Name e. -Value Invoke-Parent
New-Alias -Option ReadOnly -Name e.. -Value Invoke-Parent
function Invoke-Parent {
  Invoke-Item -Path ($PWD | Split-Path)
}

New-Alias -Option ReadOnly -Name e~ -Value Invoke-Home
function Invoke-Home {
  Invoke-Item $HOME
}

New-Alias -Option ReadOnly -Name e\ -Value Invoke-Drive
New-Alias -Option ReadOnly -Name e/ -Value Invoke-Drive
function Invoke-Drive {
  Invoke-Item $PWD.Drive.Root
}
