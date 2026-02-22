#region Override
# was: Clear-Host
Remove-Alias clear

# was: Remove-Item
Remove-Alias rd

# was: Get-Help
Remove-Alias man

# was: Get-Member
Remove-Alias gm -Force

# was: Get-ItemProperty
Remove-Alias gp -Force

# was: Get-Unique
Remove-Alias gu -Force
#endregion


#region Implicit
# was: Get-Verb (implicit)
New-Alias verb Get-VerbList -Option ReadOnly
#endregion


#region Native
# was: clip.exe
New-Alias clip Set-Clipboard -Option ReadOnly

# was: git/test.exe
New-Alias test Test-Command -Option ReadOnly
#endregion
