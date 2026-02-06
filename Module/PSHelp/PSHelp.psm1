using namespace System.Collections.Generic
using namespace Module.Completer

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

    [string[]]$Parameter
  )

  if (!$Name) {
    return Get-Help -Name Get-Help
  }

  [string]$Topic = $Name -join '_'

  $HelpContent = ''
  $ArticleUrl = [List[uri]]::new()
  $Silent = @{
    ErrorAction    = 'SilentlyContinue'
    ProgressAction = 'SilentlyContinue'
  }

  if ($CUSTOM_HELP.ContainsKey($Topic)) {
    $CustomHelp = $CUSTOM_HELP[$Topic]

    if ($CustomHelp -and $CustomHelp -as [string] -and $CustomHelp -notmatch ':') {
      $CustomHelp = $CUSTOM_HELP[[string]$CustomHelp]
    }

    if ($CustomHelp) {
      $CustomHelp |
        Where-Object {
          $PSItem -as [uri] -and (
            [uri]$PSItem
          ).IsAbsoluteUri
        } |
        ForEach-Object {
          $ArticleUrl.Add($PSItem)
        }
    }
  }
  else {
    $HelpArticleUrl = [List[uri]]::new()
    $HelpContent = Get-Help -Name $Topic @Silent

    if ($HelpContent -and $HelpContent.Count -ne 1) {
      $HelpContent = ''
    }

    if ($HelpContent) {
      $HelpContent.relatedLinks.navigationLink.Uri |
        Where-Object {
          ![string]::IsNullOrEmpty($PSItem)
        } |
        Where-Object {
          $PSItem -match '^(?=https?://\S|[-\w]+(?>\.[-\w]+)+/)'
        } |
        ForEach-Object {
          $PSItem -match '^(?=https?:)' ? $PSItem : "https://$PSItem"
        } |
        Where-Object {
          $PSItem -as [uri] -and (
            [uri]$PSItem
          ).IsAbsoluteUri
        } |
        ForEach-Object {
          $HelpArticleUrl.Add($PSItem)
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

    if ($HelpArticleUrl.Count) {
      $ArticleUrl.AddRange($HelpArticleUrl)
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

          $Test_about_Article = [uri]"$ABOUT_BASE_URL/$Topic"

          if (Test-Url -Uri $Test_about_Article) {
            return $Test_about_Article
          }
        }

        $about_Article = ''
        [string]$about_TopicCandidate = $Topic -replace '(?>[_ :]+)', '_' -replace '^(?>about)?_?', 'about_'

        $about_Article = Resolve-AboutArticle -Topic $about_TopicCandidate

        if (![string]$about_Article) {
          if ($about_TopicCandidate -notmatch 's$') {
            $about_TopicCandidate += 's'
            $about_Article = Resolve-AboutArticle -Topic $about_TopicCandidate
          }
        }

        if ([string]$about_Article) {
          $HelpContent = Get-Help -Name $about_TopicCandidate @Silent
          $ArticleUrl.Add($about_Article)
        }
      }
    }
  }

  if ($HelpContent) {
    Write-Output -InputObject $HelpContent
  }

  $Article = [List[uri]]::new()

  if ($ArticleUrl.Count) {
    $ArticleUrl.ToArray() |
      ForEach-Object {
        'https://' + $PSItem.GetComponents(
          [uricomponents]::Host -bor [uricomponents]::Path -bor (
            $PSItem.Host -eq 'go.microsoft.com' ? [uricomponents]::Query : 0
          ) -bor [uricomponents]::Fragment,
          [uriformat]::Unescaped
        )
      } |
      ForEach-Object {
        $PSItem.StartsWith(
          'https://learn.microsoft.com/en-us/',
          [stringcomparison]::OrdinalIgnoreCase
        ) ? (
          $PSItem -replace '(?<=^https://learn\.microsoft\.com/)en-us/', ''
        ) : $PSItem
      } |
      Select-Object -Unique -CaseInsensitive |
      ForEach-Object {
        $Article.Add($PSItem)
      }
  }

  if ($Article.Count) {
    Write-Information -MessageData "$($Article.ToArray() -join "`n")" -InformationAction Continue
  }

  if (!$env:SSH_CLIENT) {
    if ($Article.Count) {
      $Article.ToArray() | Open-Url
    }
    else {
      if ($HelpContent) {
        $null = Get-Help -Name $Topic -Online @Silent 2>&1
      }
    }
  }
}
