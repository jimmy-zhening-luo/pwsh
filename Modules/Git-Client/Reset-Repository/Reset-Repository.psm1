New-Alias gitr Undo-Repository
function Undo-Repository {
  param([System.String]$Path)

  Invoke-Repository -Path $Path -Verb reset --hard
}

New-Alias gitrs Restore-Repository
function Restore-Repository {
  param([System.String]$Path)

  (Undo-Repository -Path $Path) && (Get-Repository -Path $Path)
}
