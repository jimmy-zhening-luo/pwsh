Microsoft.PowerShell.Utility\New-Alias upman Update-Help

$CUSTOM_HELP_FILE = @{
  Path = "$PSScriptRoot\PSHelp.psd1"
}
$CUSTOM_HELP = (
  Test-Path @CUSTOM_HELP_FILE -Type Leaf
) ? (Import-PowerShellDataFile @CUSTOM_HELP_FILE) : @{}

Microsoft.PowerShell.Utility\New-Alias m PSTool\Get-HelpOnline
function Get-HelpOnline {
  [OutputType([Object])]
  param(
    [Parameter(
      Position = 0,
      ValueFromRemainingArguments
    )]
    [string[]]$Name,
    [string[]]$Parameter
  )

  if (-not $Name) {
    return Get-Help -Name Get-Help
  }

  $Topic = $Name -join '_'
  $Help = ''
  $HelpLink = ''
  $Articles = @()
  $Command = @{
    Name        = $Topic
    ErrorAction = 'SilentlyContinue'
  }

  if ($CUSTOM_HELP.Contains($Topic)) {
    $CustomHelp = $CUSTOM_HELP[$Topic]

    if ($CustomHelp -and $CustomHelp -notmatch ':') {
      $CustomHelp = $CUSTOM_HELP[$CustomHelp]
    }

    if ($CustomHelp) {
      $Articles += $CustomHelp
    }
  }
  else {
    $Help = Get-Help @Command

    if ($Help -and $Help.Count -gt 1) {
      $Help = ''
    }

    if ($Help) {
      $HelpLink = $Help.relatedLinks.navigationLink.Uri -replace '\?.*$', '' |
        Where-Object { $PSItem }
    }

    if ($Help -and $Parameter) {
      $ParameterHelp = Get-Help @Command -Parameter $Parameter

      if ($ParameterHelp) {
        $Help = $ParameterHelp

        if ($HelpLink -and $Parameter.Count -eq 1) {
          $HelpLink = $HelpLink + "#-$Parameter".ToLowerInvariant()
        }
      }
    }

    if ($HelpLink) {
      $Articles += $HelpLink
    }
    else {
      $ABOUT_BASE_URL = 'https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about'
      $about_Article = ''

      if ($Help) {
        $about_Article = "$ABOUT_BASE_URL/$($Help.name)"
      }
      else {
        $about_Topic = $Topic -replace '[-_ :]+', '_' -replace '^(?>about)?_?', 'about_'

        function Resolve-AboutArticle {
          param(
            [string]$Topic
          )

          $Local:about_Article = "$ABOUT_BASE_URL/$Topic"

          (Shell\Test-Url -Uri $Local:about_Article) ? $Local:about_Article : ''
        }

        $about_Article = Resolve-AboutArticle -Topic $about_Topic

        if (-not $about_Article) {
          if ($about_Topic -notmatch 's$') {
            $about_Topic += 's'
            $about_Article = Resolve-AboutArticle -Topic $about_Topic
          }
        }

        if ($about_Article) {
          $Help = Get-Help @Command -Name $about_Topic
        }
      }

      if ($about_Article) {
        $Articles += $about_Article
      }
    }
  }

  if ($Help) {
    $Help
  }

  if ($Articles) {
    $Articles = $Articles -replace '^(?>https?:\/\/)?', 'https://' -replace '^(?>https:\/\/learn\.microsoft\.com\/en-us\/)', 'https://learn.microsoft.com/' |
      Select-Object -Unique -CaseInsensitive
  }

  if (-not $env:SSH_CLIENT) {
    if ($Articles) {
      foreach ($Article in $Articles) {
        [void](Shell\Open-Url -Uri $Article)
      }
    }
    else {
      if ($Help) {
        [void](Get-Help @Command 2>&1)
      }
    }
  }

  if ($Articles) {
    $ArticleList = $Articles -join "`n"
    $ArticleInformation = @{
      MessageData       = "$ArticleList"
      InformationAction = 'Continue'
    }
    Write-Information @ArticleInformation
  }
}

Microsoft.PowerShell.Utility\New-Alias galc PSTool\Get-CommandAlias
function Get-CommandAlias {
  [OutputType([System.Management.Automation.CommandInfo])]
  param(
    [Alias('Command')]
    [string]$Definition
  )

  if (-not $Definition) {
    $Definition = '*'
  }

  $Commands = @{
    Definition = $Definition.Contains('*') ? $Definition : $Definition.Length -lt 3 ? "$Definition*" : "*$Definition*"
  }
  Get-Alias @Commands @args |
    Select-Object DisplayName, Options, Source
}

<#
.SYNOPSIS
Gets a list of approved PowerShell verbs.

.DESCRIPTION
This function gets verbs that are approved for use in PowerShell commands.

It invokes Get-Verb, sorts the returned verbs alphabetically, and returns only the Verb property as a string array.

It supports both parameters of Get-Verb, 'Verb' and 'Group', but it treats 'Verb' as a wildcard search rather than an exact match.

.PARAMETER Verb
Specifies the verb to search for. The default value is '*'. If the value contains a wildcard, it is passed to 'Get-Verb' as-is. If the value is shorter than 3 characters, a wildcard is appended ('Verb*'). If the value is 3 characters or longer, wildcards are prepended and appended ('*Verb*').

.LINK
http://learn.microsoft.com/powershell/module/microsoft.powershell.utility/get-verb

.LINK
Get-Verb
#>
function Get-VerbList {
  [OutputType([string[]])]
  param(
    [string]$Verb
  )

  if (-not $Verb) {
    $Verb = '*'
  }

  $Verbs = @{
    Verb = $Verb.Contains('*') ? $Verb : $Verb.Length -lt 3 ? "$Verb*" : "*$Verb*"
  }
  Get-Verb @Verbs @args |
    Sort-Object -Property Verb |
    Select-Object -ExpandProperty Verb
}
