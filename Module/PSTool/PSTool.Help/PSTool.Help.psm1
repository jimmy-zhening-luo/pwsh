using namespace System.Collections.Generic
using namespace System.Management.Automation

[hashtable]$CUSTOM_HELP_FILE = @{
  Path = "$PSScriptRoot\PSHelp.psd1"
}
[hashtable]$CUSTOM_HELP = (
  Test-Path @CUSTOM_HELP_FILE -Type Leaf
) ? (Import-PowerShellDataFile @CUSTOM_HELP_FILE) : @{}

[string]$ABOUT_BASE_URL = 'https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about'

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
PSTool.Help

.LINK
https://learn.microsoft.com/powershell/module/microsoft.powershell.core/get-help

.LINK
Get-Help
#>
function Get-HelpOnline {

  [CmdletBinding()]

  [OutputType([string], [System.Object])]

  param(

    [Parameter(
      Position = 0,
      ValueFromRemainingArguments
    )]
    [Completions(
      {
        [string[]]$Private:About = (Import-PowerShellDataFile -Path $PSScriptRoot\PSHelp.About.psd1).About
        [string[]]$Private:Function = ((Get-ChildItem -Path Function:).Name -notmatch [regex]'[^\w-]').ToLower()

        return $About + $Function
      },
      'Preserve'
    )]
    [string[]]$Name,

    [string[]]$Parameter

  )

  if (-not $Name) {
    return Get-Help -Name 'Get-Help'
  }

  [string]$Private:Topic = $Name -join '_'
  $Private:Help = ''
  $Private:HelpLinkArticleList = [List[uri]]::new()
  $Private:ArticleList = [List[uri]]::new()
  [hashtable]$Private:Command = @{
    Name        = $Topic
    ErrorAction = 'SilentlyContinue'
  }

  if ($CUSTOM_HELP.ContainsKey($Topic)) {
    $Private:CustomHelp = $CUSTOM_HELP[$Topic]

    if ($CustomHelp -and $CustomHelp -as [string] -and $CustomHelp -notmatch [regex]':') {
      [uri[]]$CustomHelp = $CUSTOM_HELP[[string]$CustomHelp]
    }

    if ($CustomHelp) {
      $ArticleList.AddRange(
        [List[uri]]$CustomHelp
      )
    }
  }
  else {
    $Private:Help = Get-Help @Command

    if ($Help -and $Help.Count -gt 1) {
      $Help = ''
    }

    if ($Help) {
      [string[]]$Private:HelpLinkProperty = $Help.relatedLinks.navigationLink.Uri -replace [regex]'\?(?>.*)$', '' |
        Where-Object {
          -not [string]::IsNullOrWhiteSpace($PSItem)
        } |
        Where-Object {
          $PSItem -as [uri]
        }

      if ($HelpLinkProperty) {
        $HelpLinkArticleList.AddRange(
          [List[uri]]$HelpLinkProperty
        )
      }
    }

    if ($Help -and $Parameter) {
      $Private:ParameterHelp = Get-Help @Command -Parameter $Parameter

      if ($ParameterHelp) {
        $Help = $ParameterHelp

        if ($HelpLinkArticleList.Count -eq 1 -and $Parameter.Count -eq 1) {
          [uri]$Private:CanonicalHelpLinkArticle = $HelpLinkArticleList[0]
          $HelpLinkArticleList.RemoveAt(0)

          [uri]$Private:ParameterizedCanonicalHelpLinkArticle = [string]$CanonicalHelpLinkArticle + "#-$Parameter".ToLowerInvariant()

          $HelpLinkArticleList.Add($ParameterizedCanonicalHelpLinkArticle)
        }
      }
    }

    if ($HelpLinkArticleList.Count -ne 0) {
      $ArticleList.AddRange(
        [List[uri]]$HelpLinkArticleList
      )
    }
    else {
      $Private:about_Article = ''

      if ($Help) {
        if ($Help -is [pscustomobject]) {
          [string]$Private:LocalHelpTopic = $Help.Name

          if ($LocalHelpTopic.StartsWith('about_')) {
            $about_Article = [uri]"$ABOUT_BASE_URL/$LocalHelpTopic"
          }
        }
      }
      else {
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

        [string]$Private:about_TopicCandidate = $Topic -replace [regex]'(?>[_ :]+)', '_' -replace [regex]'^(?>about)?_?', 'about_'

        $about_Article = Resolve-AboutArticle -Topic $about_TopicCandidate

        if (-not $about_Article) {
          if ($about_TopicCandidate -notmatch [regex]'s$') {
            $about_TopicCandidate += 's'
            $about_Article = Resolve-AboutArticle -Topic $about_TopicCandidate
          }
        }

        if ($about_Article) {
          $Help = Get-Help @Command -Name $about_TopicCandidate
        }
      }

      if ($about_Article) {
        $ArticleList.Add([uri]$about_Article)
      }
    }
  }

  if ($Help) {
    $Help
  }

  [string[]]$Private:Articles = $ArticleList.ToArray() -replace [regex]'^(?>https?:\/\/)?', 'https://' -replace [regex]'^https:\/\/learn\.microsoft\.com\/en-us\/', 'https://learn.microsoft.com/' |
    Select-Object -Unique -CaseInsensitive

  if ($Articles) {
    [string]$Private:ArticleDisplay = $Articles -join "`n"

    [hashtable]$Private:ArticleInformation = @{
      MessageData       = "$ArticleDisplay"
      InformationAction = 'Continue'
    }
    Write-Information @ArticleInformation
  }

  if (-not $env:SSH_CLIENT) {
    if ($Articles) {
      $Private:ArticleUrls = [List[uri]]::new(
        [List[uri]]$Articles
      )

      $ArticleUrls | Browse\Open-Url
    }
    else {
      if ($Help) {
        Get-Help @Command 2>&1 | Out-Null
      }
    }
  }
}

<#
.SYNOPSIS
Gets global aliases by command.

.DESCRIPTION
Get-Alias but Definition is the default parameter, it auto-appends/prepends wildcards, defaults to all aliases, and defaults to Global scope.

.COMPONENT
PSTool.Help

.LINK
https://learn.microsoft.com/powershell/module/microsoft.powershell.utility/get-alias

.LINK
Get-Alias
#>
function Get-CommandAlias {

  [CmdletBinding()]

  [OutputType([System.Management.Automation.CommandInfo[]])]

  param(

    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [SupportsWildcards()]
    [Alias('Command')]
    [Completions({ return @('*') })]
    # Gets the aliases for the specified item. Enter the name of a cmdlet, function, script, file, or executable file. This parameter is called Definition, because it searches for the item name in the Definition property of the alias object.
    [string[]]$Definition,

    [Parameter(
      Position = 1
    )]
    [SupportsWildcards()]
    [Completions(
      {
        return @(
          'Global'
          'Local'
          'Script'
          '0'
          '1'
          '2'
          '3'
        )
      }
    )]
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
      $AliasList.AddRange(
        [List[CommandInfo]]$AliasResults
      )
    }
  }

  end {
    [CommandInfo[]]$Private:UniqueAliases = $AliasList.ToArray() |
      Sort-Object -Property DisplayName |
      Group-Object -Property DisplayName |
      ForEach-Object {
        $PSItem.Group[0]
      }

    return $UniqueAliases
  }
}

enum VerbGroup {
  Communications
  Data
  Diagnostic
  Lifecycle
  Security
  Service
  Settings
  Support
  System
  Utility
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
PSTool.Help

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
    [Completions({ return @('*') })]
    # Gets only the specified verbs. Enter the name of a verb or a name pattern. Wildcards are allowed.
    [string[]]$Verb,

    [Parameter(
      Position = 1
    )]
    [Completions(
      {
        return [VerbGroup].GetEnumNames()
      }
    )]
    # Gets only the specified group. Enter the name of a group. Wildcards aren't allowed.
    [string]$Group

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
      $VerbList.AddRange(
        [List[string]]$VerbResults
      )
    }
  }

  end {
    $VerbList.Sort()

    [string[]]$Private:UniqueVerbs = $VerbList |
      Select-Object -Unique -CaseInsensitive

    return $UniqueVerbs
  }
}

New-Alias upman Update-Help
New-Alias psk Get-PSReadLineKeyHandler

New-Alias m Get-HelpOnline
New-Alias galc Get-CommandAlias
