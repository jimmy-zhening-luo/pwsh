# Core
Import-Module "$PSScriptRoot\Invoke-Repository"
Export-ModuleMember Invoke-Repository, Resolve-Repository -Alias gitc

# Verbs
Import-Module "$PSScriptRoot\Verbs\Add-Repository"
Export-ModuleMember Add-Repository -Alias gita

Import-Module "$PSScriptRoot\Verbs\Clone-Repository"
Export-ModuleMember Import-Repository -Alias gitcl

Import-Module "$PSScriptRoot\Verbs\Commit-Repository"
Export-ModuleMember Write-Repository -Alias gitm

Import-Module "$PSScriptRoot\Verbs\Pull-Repository"
Export-ModuleMember Get-Repository, Get-ChildRepository -Alias gitp, gitpa

Import-Module "$PSScriptRoot\Verbs\Push-Repository"
Export-ModuleMember Push-Repository -Alias gitcp

Import-Module "$PSScriptRoot\Verbs\Reset-Repository"
Export-ModuleMember Undo-Repository, Restore-Repository -Alias gitcr, gitcrp
