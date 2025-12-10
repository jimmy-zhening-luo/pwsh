using namespace System.Collections.Generic

New-Alias upman Update-Help

$CUSTOM_HELP_FILE = @{
  Path = "$PSScriptRoot\PSHelp.psd1"
}
$CUSTOM_HELP = (
  Test-Path @CUSTOM_HELP_FILE -Type Leaf
) ? (Import-PowerShellDataFile @CUSTOM_HELP_FILE) : @{}

New-Alias m Get-HelpOnline
function Get-HelpOnline {
  [OutputType([System.Object])]
  param(
    [Parameter(
      Position = 0,
      ValueFromRemainingArguments
    )]
    [string[]]$Name,
    [string[]]$Parameter
  )

  if (-not $Name) {
    return Get-Help -Name 'Get-Help'
  }

  [string]$Private:Topic = $Name -join '_'
  $Private:Help = ''
  $Private:HelpLinkArticles = [List[uri]]::new()
  $Private:Articles = [List[uri]]::new()
  [hashtable]$Private:Command = @{
    Name        = $Topic
    ErrorAction = 'SilentlyContinue'
  }

  if ($CUSTOM_HELP.Contains($Topic)) {
    $Private:CustomHelp = $CUSTOM_HELP[$Topic]

    if ($CustomHelp -and $CustomHelp -as [string] -and $CustomHelp -notmatch ':') {
      [uri[]]$CustomHelp = $CUSTOM_HELP[[string]$CustomHelp]
    }

    if ($CustomHelp) {
      $Articles.AddRange([List[uri]]$CustomHelp)
    }
  }
  else {
    $Private:Help = Get-Help @Command

    if ($Help -and $Help.Count -gt 1) {
      $Help = ''
    }

    if ($Help) {
      [string[]]$Private:HelpLinkProperty = $Help.relatedLinks.navigationLink.Uri -replace '\?(?>.*)$', '' |
        Where-Object { $PSItem } |
        Where-Object { $PSItem -as [uri] }

      if ($HelpLinkProperty) {
        $HelpLinkArticles.AddRange([List[uri]]$HelpLinkProperty)
      }
    }

    if ($Help -and $Parameter) {
      $Private:ParameterHelp = Get-Help @Command -Parameter $Parameter

      if ($ParameterHelp) {
        $Help = $ParameterHelp

        if ($HelpLinkArticles.Count -eq 1 -and $Parameter.Count -eq 1) {
          [uri]$Private:CanonicalHelpLinkArticle = $HelpLinkArticles[0]
          $HelpLinkArticles.RemoveAt(0)

          [uri]$Private:ParameterizedCanonicalHelpLinkArticle = [string]$CanonicalHelpLinkArticle + "#-$Parameter".ToLowerInvariant()

          $HelpLinkArticles.Add($ParameterizedCanonicalHelpLinkArticle)
        }
      }
    }

    if ($HelpLinkArticles.Count -ne 0) {
      $Articles.AddRange([List[uri]]$HelpLinkArticles)
    }
    else {
      [string]$Private:ABOUT_BASE_URL = 'https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about'
      [string]$Private:about_Article = ''

      if ($Help) {
        $about_Article = [uri]"$ABOUT_BASE_URL/$($Help.name)"
      }
      else {
        [string]$Private:about_Topic = $Topic -replace '(?>[-_ :]+)', '_' -replace '^(?>about)?_?', 'about_'

        function Resolve-AboutArticle {
          [OutputType([uri])]
          param(
            [string]$Topic
          )

          [uri]$Private:about_Article = "$ABOUT_BASE_URL/$Topic"

          if (Browse\Test-Url -Uri $about_Article) {
            return $about_Article
          }
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
        $Articles.Add([uri]$about_Article)
      }
    }
  }

  if ($Help) {
    $Help
  }

  [string[]]$Private:ArticleList = $Articles.ToArray()

  if ($ArticleList) {
    $ArticleList = $ArticleList -replace '^(?>https?:\/\/)?', 'https://' -replace '^https:\/\/learn\.microsoft\.com\/en-us\/', 'https://learn.microsoft.com/' |
      Select-Object -Unique -CaseInsensitive

    [string]$Private:ArticleDisplay = $ArticleList -join "`n"

    [hashtable]$Private:ArticleInformation = @{
      MessageData       = "$ArticleDisplay"
      InformationAction = 'Continue'
    }
    Write-Information @ArticleInformation
  }

  if (-not $env:SSH_CLIENT) {
    if ($ArticleList) {
      foreach ($Private:Article in $ArticleList) {
        Browse\Open-Url -Uri [uri]$Article
      }
    }
    else {
      if ($Help) {
        Get-Help @Command 2>&1 | Out-Null
      }
    }
  }
}

New-Alias galc Get-CommandAlias
function Get-CommandAlias {
  [OutputType([System.Management.Automation.CommandInfo])]
  param(
    [Alias('Command')]
    [string]$Definition
  )

  if (-not $Definition) {
    $Definition = '*'
  }

  [hashtable]$Private:Commands = @{
    Definition = $Definition.Contains('*') ? $Definition : $Definition.Length -lt 3 ? "$Definition*" : "*$Definition*"
  }
  [hashtable]$Private:Property = @{
    Property = @(
      'DisplayName'
      'Options'
      'Source'
    )
  }
  return Get-Alias @Commands @args |
    Select-Object @Property
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

  [hashtable]$Private:Verbs = @{
    Verb = $Verb.Contains('*') ? $Verb : $Verb.Length -lt 3 ? "$Verb*" : "*$Verb*"
  }
  return Get-Verb @Verbs @args |
    Sort-Object -Property Verb |
    Select-Object -ExpandProperty Verb
}
