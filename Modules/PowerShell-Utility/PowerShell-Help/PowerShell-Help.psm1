New-Alias upman Update-Help

$CUSTOM_HELP_ARTICLE_PATH = @{
  Path = Join-Path $PSScriptRoot 'PowerShell-HelpArticle.psd1'
}
$CUSTOM_HELP_ARTICLE = (
  Test-Path @CUSTOM_HELP_ARTICLE_PATH -Type Leaf
) ? (Import-PowerShellDataFile @CUSTOM_HELP_ARTICLE_PATH) : @{}
$ONLINE_HELP_ROOT = 'https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about'

New-Alias m Get-HelpOnline
<#
.FORWARDHELPTARGETNAME Get-Help
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-HelpOnline {
  param(
    [string[]]$Name,
    [Alias('params', 'args', 'Argument')]
    [string[]]$Parameter
  )

  if (-not $Name) {
    Get-Help -Name Get-Help
    return
  }

  $Help = $null
  $HelpUri = $null
  $Articles = @()
  $Suppress = @{
    ErrorAction = 'SilentlyContinue'
  }

  $FullName = $Name -join '_'

  if ($CUSTOM_HELP_ARTICLE.Contains($FullName)) {
    $Record = $CUSTOM_HELP_ARTICLE[$FullName]

    if ($Record -is [string] -and -not $Record.Contains(':')) {
      $Record = $CUSTOM_HELP_ARTICLE[$Record]
    }

    if ($Record) { $Articles += $Record }
  }
  else {
    $Help = Get-Help -Name $FullName @Suppress
    $HelpUri = $Help.relatedLinks.navigationLink.Uri |
      ? { -not [string]::IsNullOrEmpty($_) } |
      % { $_ -replace '\?.*$', '' } |
      ? { $_ -ne '' }

    if ($HelpUri) {
      $Articles += $HelpUri
    }
    else {
      $about_Name = $FullName -replace '[-_ :]+', '_' -replace '^(?:about)?_?', 'about_'
      $about_Article = "$ONLINE_HELP_ROOT/$about_Name"

      if (Test-Url $about_Article) {
        $Articles += $about_Article
      }
      elseif ($about_Name -notmatch 's$' -and (Test-Url ($about_Article + 's'))) {
        $Articles += $about_Article + 's'
      }
    }

    if ($Help -and $Parameter) {
      $ParameterHelp = Get-Help -Name $FullName -Parameter $Parameter @Suppress

      if ($ParameterHelp) {
        $Help = $ParameterHelp

        if ($HelpUri -and $Parameter.Count -eq 1) {
          $Articles[-1] = $HelpUri + "#-$Parameter".ToLowerInvariant()
        }
      }
    }
  }

  if ($Help) { $Help }

  if ($Articles) {
    "`r`nDOCS"
    $Articles |
      % { $_ -replace '^(?:https?:\/\/)?', 'https://' } |
      % { $_ -replace '^https:\/\/learn\.microsoft\.com\/en-us\/', 'https://learn.microsoft.com/' }
  }

  if (-not $env:SSH_CLIENT) {
    if ($Articles) {
      foreach ($Article in $Articles) {
        Open-Url -Uri $Article
      }
    }
    else {
      $ErrorMessage = Get-Help -Name $FullName -Online 2>&1

      if ($ErrorMessage) {
        Write-Warning "No online help found for topic '$FullName'"
      }
    }
  }
}
