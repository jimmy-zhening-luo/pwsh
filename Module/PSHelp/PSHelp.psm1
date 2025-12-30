using namespace System.Collections.Generic
using namespace Completer

$CUSTOM_HELP = Import-PowerShellDataFile -Path $PSScriptRoot\PSHelpTopic.psd1
$ABOUT_BASE_URL = 'https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about'

<#
.SYNOPSIS
Displays information about PowerShell commands and concepts, opening the relevant online documentation page in Chrome if available.

.DESCRIPTION
This function retrieves help information for the specified command or topic. If local help is available, it displays that help in the console.

If online documentation is available, it opens the relevant page in the default web browser if not in an SSH session. It also writes the URLs of the documentation pages to the information stream without returning it as output.

Online documentation is determined based on the related links in the local help, by constructing URLs for about_ articles, or by configured custom help links in .\PSHelp.psd1.

.PARAMETER Name
Gets help about the specified command or concept. Enter the name of a cmdlet, function, provider, script, or workflow, such as Get-Member, a conceptual article name, such as about_Objects, or an alias, such as ls. Wildcard characters are permitted in cmdlet and provider names, but you can't use wildcard characters to find the names of function help and script help articles.

To get help for a script that isn't located in a path that's listed in the $Env:PATH environment variable, type the script's path and file name.

If you enter the exact name of a help article, Get-Help displays the article contents.

The names of conceptual articles, such as about_Objects, must be entered in English, even in non-English versions of PowerShell.

The command or topic name can be provided as a string array, which will be joined with underscores to form the full name.

If you don't specify a name, Get-Help displays local help about the Get-Help cmdlet.

.PARAMETER Parameter
One or more parameters can also be specified to get help for specific parameters of the command. When parameters are specified, the displayed local help will only include information about those parameters.

If a single parameter is specified and there is exactly one online documentation link available, the parameter name will be appended to the link as a fragment identifier as a best-effort attempt to direct the user to the relevant section of the documentation.

.COMPONENT
PSHelp

.LINK
https://learn.microsoft.com/powershell/module/microsoft.powershell.core/get-help

.LINK
Get-Help
#>
function Get-HelpOnline {
  [CmdletBinding()]
  [OutputType([string], [System.Object])]
  [Alias('m', 'man')]
  param(

    [Parameter(
      Position = 0,
      ValueFromRemainingArguments
    )]
    [DynamicCompletions(
      {
        return (
          Import-PowerShellDataFile -Path $PSScriptRoot\PSHelpTopic.Local.psd1
        ).About + (
          (
            Get-ChildItem -Path Function:
          ).Name.ToLower() -notmatch '[^\w-]'
        ).ToLower()
      }
    )]
    [string[]]$Name,

    [string[]]$Parameter,

    [Parameter(DontShow)][switch]$z
  )

  if (-not $Name) {
    return Get-Help -Name Get-Help
  }

  [string]$Topic = $Name -join '_'
  $ArticleUrl = [List[uri]]::new()
  $HelpContent = [string]::Empty
  $Silent = @{
    ErrorAction    = 'SilentlyContinue'
    ProgressAction = 'SilentlyContinue'
  }

  if ($CUSTOM_HELP.ContainsKey($Topic)) {
    $CustomHelp = $CUSTOM_HELP[$Topic]

    if ($CustomHelp -and $CustomHelp -as [string] -and $CustomHelp -notmatch ':') {
      $CustomHelp = $CUSTOM_HELP[[string]$CustomHelp]
    }

    [string[]]$CustomHelpString = @()

    if ($CustomHelp) {
      $CustomHelpString += $CustomHelp |
        Where-Object {
          $PSItem -as [uri] -and (
            [uri]$PSItem
          ).IsAbsoluteUri
        }
    }

    if ($CustomHelpString) {
      $ArticleUrl.AddRange(
        [List[uri]]$CustomHelpString
      )
    }
  }
  else {
    $HelpArticleUrl = [List[uri]]::new()
    $HelpContent = Get-Help -Name $Topic @Silent

    if ($HelpContent -and $HelpContent.Count -gt 1) {
      $HelpContent = [string]::Empty
    }

    if ($HelpContent) {
      [uri[]]$RelatedUrl = $HelpContent.relatedLinks.navigationLink.Uri |
        Where-Object {
          -not [string]::IsNullOrEmpty($PSItem)
        } |
        Where-Object {
          $PSItem -match '^(?=(?>https?://\S|(?>[-\w]+)(?>\.(?>[-\w]+))+/))'
        } |
        ForEach-Object {
          $PSItem -match '^(?=https?:)' ? $PSItem : "https://$PSItem"
        } |
        Where-Object {
          $PSItem -as [uri] -and (
            [uri]$PSItem
          ).IsAbsoluteUri
        }

      if ($RelatedUrl.Count -ne 0) {
        $HelpArticleUrl.AddRange(
          [List[uri]]$RelatedUrl
        )
      }
    }

    if ($HelpContent -and $Parameter) {
      $ParameterHelpContent = Get-Help -Name $Topic -Parameter $Parameter @Silent

      if ($ParameterHelpContent) {
        $HelpContent = $ParameterHelpContent

        if ($HelpArticleUrl.Count -eq 1 -and $Parameter.Count -eq 1) {
          $HelpArticleUrl[0] = [uri](
            [string]$HelpArticleUrl[0] + "#-$Parameter".ToLower()
          )
        }
      }
    }

    if ($HelpArticleUrl.Count -ne 0) {
      $ArticleUrl.AddRange(
        [List[uri]]$HelpArticleUrl
      )
    }
    else {
      if ($HelpContent) {
        if (
          $HelpContent.Name -as [string] -and (
            [string]$HelpContent.Name
          ).StartsWith(
            'about_'
          )
        ) {
          $about_Article = [uri]"$ABOUT_BASE_URL/$([string]$HelpContent.Name)"
        }
      }
      else {
        function Resolve-AboutArticle {
          [CmdletBinding()]
          [OutputType([uri])]
          param(
            [Parameter()]
            [string]$Topic
          )

          [uri]$about_Article = "$ABOUT_BASE_URL/$Topic"

          if (Test-Url -Uri $about_Article) {
            return $about_Article
          }
        }

        $about_Article = [string]::Empty
        [string]$about_TopicCandidate = $Topic -replace '(?>[_ :]+)', '_' -replace '^(?>about)?_?', 'about_'

        $about_Article = Resolve-AboutArticle -Topic $about_TopicCandidate

        if (-not $about_Article) {
          if ($about_TopicCandidate -notmatch 's$') {
            $about_TopicCandidate += 's'
            $about_Article = Resolve-AboutArticle -Topic $about_TopicCandidate
          }
        }

        if ($about_Article) {
          $HelpContent = Get-Help -Name $about_TopicCandidate @Silent
          $ArticleUrl.Add([uri]$about_Article)
        }
      }
    }
  }

  if ($HelpContent) {
    Write-Output -InputObject $HelpContent
  }

  $Article = [List[string]]::new()

  if ($ArticleUrl.Count -ne 0) {
    [string[]]$ArticalString = $ArticleUrl |
      ForEach-Object {
        'https://' + $PSItem.Host + (
          $PSItem.Host -eq 'go.microsoft.com' ? $PSItem.PathAndQuery : $PSItem.AbsolutePath.Substring(
            $PSItem.AbsolutePath.StartsWith(
              '/en-us/',
              [stringcomparison]::OrdinalIgnoreCase 
            ) ? 6 : 0
          )
        ) + $PSItem.Fragment
      } |
      Select-Object -Unique -CaseInsensitive

    if ($ArticleString) {
      $Article.AddRange(
        [List[string]]$ArticleString
      )
    }
  }

  if ($Article.Count -ne 0) {
    Write-Information -MessageData "$($Article -join "`n")" -InformationAction Continue
  }

  if (-not $env:SSH_CLIENT) {
    if ($Article.Count -eq 0) {
      if ($HelpContent) {
        Get-Help -Name $Topic -Online @Silent 2>&1 | Out-Null
      }
    }
    else {
      [uri[]]$Article | Browse\Open-Url
    }
  }
}

<#
.SYNOPSIS
Gets global aliases by command.

.DESCRIPTION
Get-Alias but Definition is the default parameter, it auto-appends/prepends wildcards, defaults to all aliases, and defaults to Global scope.

.COMPONENT
PSHelp

.LINK
https://learn.microsoft.com/powershell/module/microsoft.powershell.utility/get-alias

.LINK
Get-Alias
#>
function Get-CommandAlias {
  [CmdletBinding()]
  [OutputType([System.Management.Automation.CommandInfo])]
  [Alias('galc')]
  param(

    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [SupportsWildcards()]
    [Alias('Command')]
    [Completions('*')]
    # Gets the aliases for the specified item. Enter the name of a cmdlet, function, script, file, or executable file. This parameter is called Definition, because it searches for the item name in the Definition property of the alias object.
    [string[]]$Definition,

    [Parameter(
      Position = 1
    )]
    [SupportsWildcards()]
    [Completions(
      'global,local,script,0,1,2,3'
    )]
    # Specifies the scope for which this cmdlet gets aliases. The acceptable values for this parameter are: Global, Local, Script, and a positive integer relative to the current scope (0 through the number of scopes, where 0 is the current scope and 1 is its parent). Global is the default, which differs from Get-Alias where Local is the default.
    [string]$Scope,

    [Parameter(
      Position = 2
    )]
    [SupportsWildcards()]
    # Omits the specified items. The value of this parameter qualifies the Definition parameter. Enter a name, a definition, or a pattern, such as "s*". Wildcards are permitted.
    [string[]]$Exclude,

    [Parameter(DontShow)][switch]$z
  )

  begin {
    if (-not $Scope) {
      $Scope = 'Global'
    }

    $AliasList = [List[System.Management.Automation.CommandInfo]]::new()
  }

  process {
    $AliasQuery = @{
      Scope      = $Scope
      Definition = $Definition ? $Definition.Contains([char]'*') ? $Definition : $Definition.Length -lt 3 ? "$Definition*" : "*$Definition*" : '*'
    }

    if ($Exclude) {
      $AliasQuery.Exclude = $Exclude
    }

    [System.Management.Automation.CommandInfo[]]$AliasResults = Get-Alias @AliasQuery

    if ($AliasResults) {
      $AliasList.AddRange(
        [List[System.Management.Automation.CommandInfo]]$AliasResults
      )
    }
  }

  end {
    return $AliasList |
      Sort-Object -Property DisplayName |
      Group-Object -Property DisplayName |
      ForEach-Object {
        $PSItem.Group[0]
      }
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

.COMPONENT
PSHelp

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
    [Completions('*')]
    # Gets only the specified verbs. Enter the name of a verb or a name pattern. Wildcards are allowed.
    [string[]]$Verb,

    [Parameter(
      Position = 1
    )]
    [Completions(
      'communications,data,diagnostic,lifecycle,security,service,settings,support,system,utility'
    )]
    # Gets only the specified group. Enter the name of a group. Wildcards aren't allowed.
    [string]$Group,

    [Parameter(DontShow)][switch]$z
  )

  begin {
    $VerbList = [List[string]]::new()
  }

  process {
    $VerbQuery = @{
      Verb = $Verb ? $Verb.Contains([char]'*') ? $Verb : $Verb.Length -lt 3 ? "$Verb*" : "*$Verb*" : '*'
    }

    if ($Group) {
      $VerbQuery.Group = $Group
    }

    [string[]]$VerbResults = Get-Verb @VerbQuery |
      Select-Object -ExpandProperty Verb

    if ($VerbResults) {
      $VerbList.AddRange(
        [List[string]]$VerbResults
      )
    }
  }

  end {
    $VerbList.Sort()

    return $VerbList |
      Select-Object -Unique -CaseInsensitive
  }
}

New-Alias psk PSReadLine\Get-PSReadLineKeyHandler
New-Alias upman Update-Help
