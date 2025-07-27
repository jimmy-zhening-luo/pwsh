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

  $Articles = @()

  if ($Name) {
    $ErrorStop = @{ ErrorAction = "Stop" }

    if ($HELP.Contains($Name)) {
      $HELP[$Name] |
        ForEach-Object {
          $Articles += $_
          Open-Url $_
        }
    }
    else {
      try {
        $Articles += (
          (
            Get-Help $Name @ErrorStop
          ).relatedLinks.navigationLink.Uri |
            Where-Object { $_ -ne "" } |
            ForEach-Object { $_ -replace "\?.*$", "" }
        )
        Get-Help $Name -Online -ErrorAction Stop | Out-Null
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
    }

    if ($Parameter) {
      try {
        Get-Help $Name -Parameter $Parameter @ErrorStop
      }
      catch {
        try {
          Get-Help $Name @ErrorStop
          Write-Warning "No offline help found for parameters '$Parameter' in topic '$Name'."
        }
        catch {
          throw "No offline help found for topic '$Name'."
        }
      }
    }
    else {
      try {
        Get-Help $Name @ErrorStop
      }
      catch {
        throw "No offline help found for topic '$Name'."
      }
    }
  }
  else {
    Get-Help Get-Help -Online
    Get-Help Get-Help
    Write-Warning "No help topic specified, showing Get-Help by default."
  }

  if ($Local:Articles.Count -gt 0) {
    "`r`nOnline Help:"
    $Articles |
      ForEach-Object { $_ -replace "^https?:\/\/", "http://" } |
      ForEach-Object { $_ -replace "^learn\.microsoft\.com\/en-us\/", "^learn.microsoft.com/" }
  }
  else {
    Write-Warning "No online help found."
  }
}
