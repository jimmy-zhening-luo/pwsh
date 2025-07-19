New-Alias -Name parent -Value Get-Parent -Option ReadOnly
function Get-Parent {
  param(
    [Parameter(ValueFromPipeline)]
    [string]$Path = "."
  )
  process {
    Split-Path $Path -Resolve
  }
}

New-Alias -Name size -Value Get-FileSize -Option ReadOnly
function Get-FileSize {
  param(
    [Parameter(ValueFromPipeline)]
    [string]$Path = "."
  )
  process {
    if (Test-Path $Path) {
      if ((Get-Item $Path).PSIsContainer) {
        (
          Get-ChildItem -Path $Path -Recurse -File
          | Measure-Object -Property Length -Sum
        ).Sum
      }
      else {
        (Get-Item $Path).Length
      }
    }
    else {
      throw "'$Path' is not a path."
    }
  }
}
