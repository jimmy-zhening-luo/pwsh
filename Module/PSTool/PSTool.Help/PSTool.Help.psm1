using namespace System.Collections.Generic
using namespace System.Management.Automation

New-Alias upman Update-Help

$CUSTOM_HELP_FILE = @{
  Path = "$PSScriptRoot\PSHelp.psd1"
}
$CUSTOM_HELP = (
  Test-Path @CUSTOM_HELP_FILE -Type Leaf
) ? (Import-PowerShellDataFile @CUSTOM_HELP_FILE) : @{}

New-Alias m Get-HelpOnline
function Get-HelpOnline {
  [CmdletBinding()]
  [OutputType([string], [System.Object])]
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
  [CmdletBinding()]
  [OutputType([CommandInfo[]])]
  param(
    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [SupportsWildcards()]
    [GenericCompletions('*')]
    [Alias('Command')]
    # Gets the aliases for the specified item. Enter the name of a cmdlet, function, script, file, or executable file. This parameter is called Definition, because it searches for the item name in the Definition property of the alias object.
    [string[]]$Definition,
    [Parameter(
      Position = 1
    )]
    [SupportsWildcards()]
    [GenericCompletions('Global,Local,Script,0,1,2,3')]
    # Specifies the scope for which this cmdlet gets aliases. The acceptable values for this parameter are: Global, Local, Script, and a positive integer relative to the current scope (0 through the number of scopes, where 0 is the current scope and 1 is its parent). Global is the default, which differs from Get-Alias where Local is the default.
    [string]$Scope,
    [Parameter(
      Position = 2
    )]
    [SupportsWildcards()]
    # Omits the specified items. The value of this parameter qualifies the Definition parameter. Enter a name, a definition, or a pattern, such as "s*". Wildcards are permitted.
    [string[]]$Exclude
  )

  begin {
    if (-not $Scope) {
      $Scope = 'Global'
    }

    $Private:AliasList = [List[CommandInfo]]::new()
  }
  process {
    [hashtable]$Private:AliasQuery = @{
      Scope      = $Scope
      Definition = $Definition ? $Definition.Contains('*') ? $Definition : $Definition.Length -lt 3 ? "$Definition*" : "*$Definition*" : '*'
    }

    if ($Exclude) {
      $AliasQuery.Exclude = $Exclude
    }

    [CommandInfo[]]$Private:AliasResults = Get-Alias @Private:AliasQuery

    if ($AliasResults) {
      $AliasList.AddRange([List[CommandInfo]]$AliasResults)
    }
  }
  end {
    [CommandInfo[]]$Private:UniqueAliases = $AliasList.ToArray() |
      Sort-Object -Property 'DisplayName' |
      Group-Object -Property 'DisplayName' |
      ForEach-Object {
        $PSItem.Group[0]
      }

    return $UniqueAliases
  }
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
  [CmdletBinding()]
  [OutputType([string[]])]
  param(
    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [SupportsWildcards()]
    [GenericCompletions('*')]
    # Gets only the specified verbs. Enter the name of a verb or a name pattern. Wildcards are allowed.
    [string[]]$Verb,
    [Parameter(
      Position = 1,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [GenericCompletions('Common,Communications,Data,Diagnostic,Lifecycle,Other,Security')]
    # Gets only the specified groups. Enter the name of a group. Wildcards aren't allowed.
    [string[]]$Group
  )

  begin {
    $Private:VerbList = [List[string]]::new()
  }
  process {
    [hashtable]$Private:VerbQuery = @{
      Verb = $Verb ? $Verb.Contains('*') ? $Verb : $Verb.Length -lt 3 ? "$Verb*" : "*$Verb*" : '*'
    }

    if ($Group) {
      $VerbQuery.Group = $Group
    }

    [string[]]$Private:VerbResults = Get-Verb @VerbQuery |
      Select-Object -ExpandProperty Verb

    if ($VerbResults) {
      $VerbList.AddRange([List[string]]$VerbResults)
    }
  }
  end {
    $VerbList.Sort()

    [string[]]$Private:UniqueVerbs = $VerbList |
      Select-Object -Unique -CaseInsensitive

    return $UniqueVerbs
  }
}
