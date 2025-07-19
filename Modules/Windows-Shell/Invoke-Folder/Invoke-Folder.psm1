New-Alias explore Invoke-Folder
New-Alias e Invoke-Folder
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

New-Alias e. Invoke-Parent
New-Alias e.. Invoke-Parent
function Invoke-Parent {
  Invoke-Item -Path ($PWD | Split-Path)
}

New-Alias e~ Invoke-Home
function Invoke-Home {
  Invoke-Item $HOME
}

New-Alias e\ Invoke-Drive
New-Alias e/ Invoke-Drive
function Invoke-Drive {
  Invoke-Item $PWD.Drive.Root
}
