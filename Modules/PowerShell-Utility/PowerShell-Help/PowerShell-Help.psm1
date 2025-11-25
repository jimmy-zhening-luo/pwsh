New-Alias upman Update-Help

$CUSTOM_LINKS_PATH = @{
  Path = Join-Path $PSScriptRoot 'PowerShell-HelpArticle.psd1'
}
$CUSTOM_LINKS = (
  Test-Path @CUSTOM_LINKS_PATH -Type Leaf
) ? (Import-PowerShellDataFile @CUSTOM_LINKS_PATH) : @{}
$ABOUT_ARTICLE_ROOT = 'https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about'

New-Alias m Get-HelpOnline
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

  $Topic = $Name -join '_'
  $Query = @{
    Name        = $Topic
    ErrorAction = 'SilentlyContinue'
  }
  $Help = ''
  $HelpLink = ''
  $Articles = @()

  if ($CUSTOM_LINKS.Contains($Topic)) {
    $CustomLink = $CUSTOM_LINKS[$Topic]

    if ($CustomLink -is [string] -and -not $CustomLink.Contains(':')) {
      $CustomLink = $CUSTOM_LINKS[$CustomLink]
    }

    if ($CustomLink) {
      $Articles += $CustomLink
    }
  }
  else {
    $Help = Get-Help @Query

    if ($Help -and $Help.Count -gt 1) {
      $Help = ''
    }

    if ($Help) {
      $HelpLink = $Help.relatedLinks.navigationLink.Uri |
        ? { -not [string]::IsNullOrEmpty($_) } |
        % { $_ -replace '\?.*$', '' } |
        ? { $_ -ne '' }
    }

    if ($Help -and $Parameter) {
      $ParameterHelp = Get-Help @Query -Parameter $Parameter

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
      $about_Article = ''

      if ($Help) {
        $about_Article = "$ABOUT_ARTICLE_ROOT/$($Help.name)"
      }
      else {
        $about_Topic = $Topic -replace '[-_ :]+', '_' -replace '^(?:about)?_?', 'about_'

        function Resolve-AboutArticle {
          param([string]$about_Topic)

          $about_Article = "$ABOUT_ARTICLE_ROOT/$about_Topic"

          (Test-Url $about_Article) ? $about_Article : ''
        }

        $about_Article = Resolve-AboutArticle $about_Topic

        if (-not $about_Article) {
          if ($about_Topic -notmatch 's$') {
            $about_Topic = $about_Topic + 's'
            $about_Article = Resolve-AboutArticle $about_Topic
          }
        }

        if ($about_Article) {
          $Help = Get-Help @Query -Name $about_Topic
        }
      }

      if ($about_Article) {
        $Articles += $about_Article
      }
    }
  }

  if ($Help) { $Help }

  if ($Articles) {
    $Articles = $Articles |
      % { $_ -replace '^(?:https?:\/\/)?', 'https://' } |
      % { $_ -replace '^https:\/\/learn\.microsoft\.com\/en-us\/', 'https://learn.microsoft.com/' } |
      Select-Object -Unique
  }

  if (-not $env:SSH_CLIENT) {
    if ($Articles) {
      foreach ($Article in $Articles) {
        [void](Open-Url -Uri $Article)
      }
    }
    else {
      if ($Help) {
        [void](Get-Help @Query 2>&1)
      }
    }
  }

  if ($Articles) { $Articles }
}
