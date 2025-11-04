New-Alias upman Update-Help

New-Alias m Get-HelpOnline
<#
.FORWARDHELPTARGETNAME Get-Help
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-HelpOnline {
  param(
    [Parameter(Position = 0)]
    [string]$Name,
    [Parameter(ValueFromRemainingArguments)]
    [string[]]$Parameter
  )

  if ($Name) {
    function Show-Help {
      Get-Help -Name $Name -ErrorAction Stop @args
    }

    $Articles = @()

    try {
      $Articles += (
        (Show-Help).relatedLinks.navigationLink.Uri |
          ? { $_ -ne "" } |
          % { $_ -replace "\?.*$", "" }
      )
      Show-Help -Online | Out-Null
    }
    catch {
      $NameLower = $Name.ToLowerInvariant()
      $about_Name = $NameLower.StartsWith("about_") ? $Name : $NameLower.StartsWith("about") ? ("about_" + $Name.Substring(5)) : $Name.StartsWith("_") ? "about$Name" : "about_$Name"
      $AboutArticle = "https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about/$about_Name"

      if (Test-Url $AboutArticle) {
        $Articles += $AboutArticle
        Open-Url $AboutArticle
      }
      elseif (-not $AboutArticle.EndsWith("s") -and (Test-Url ($AboutArticle + "s"))) {
        $Articles += $AboutArticle + "s"
        Open-Url ($AboutArticle + "s")
      }
    }


    if ($Parameter) {
      try {
        Show-Help -Parameter $Parameter
      }
      catch {
        try {
          Show-Help
          Write-Warning "No offline help found for parameters '$Parameter' in topic '$Name'."
        }
        catch {
          throw "No offline help found for topic '$Name'."
        }
      }
    }
    else {
      try {
        Show-Help
      }
      catch {
        throw "No offline help found for topic '$Name'."
      }
    }

    if ($Local:Articles.Count -gt 0) {
      "`r`nDOCS"
      $Articles |
        % { $_ -replace "^https?:\/\/", "http://" } |
        % { $_ -replace "^learn\.microsoft\.com\/en-us\/", "^learn.microsoft.com/" }
    }
    else {
      Write-Warning "No online help found."
    }
  }
  else {
    Get-Help -Name Get-Help -Online
    Get-Help -Name Get-Help
  }
}
