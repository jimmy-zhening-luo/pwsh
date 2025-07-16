New-Alias galc Get-AliasCommand
function Get-AliasCommand {
  param([string]$Definition)

  $Splat = $Definition ? @{
    Definition = (
      ($Definition.Length -lt 3) ? "" : "*"
    ) + $Definition + "*"
  } : @{}

  Get-Alias @Splat | Select-Object DisplayName
}

New-Alias verb Get-VerbList
function Get-VerbList {
  Get-Verb | Sort-Object -Property Verb | Select-Object Verb
}

New-Alias help Get-Help
New-Alias mano Get-HelpOnline
New-Alias m Get-HelpOnline
New-Alias upman Update-Help
function Get-HelpOnline {
  param(
    [Parameter(Position = 0)]
    [string]$Name,
    [Parameter(ValueFromRemainingArguments)]
    [string[]]$Parameter
  )

  if ($Name) {
    $Articles = @()
    $Splat = @{
      Name        = $Name
      ErrorAction = "Stop"
    }

    if ($HELP.Contains($Name)) {
      $HELP[$Name]
      | ForEach-Object {
        $Articles += $_
        Open-Url $_
      }
    }
    else {
      try {
        $Articles += (
          (
            Get-Help @Splat
          ).relatedLinks.navigationLink.Uri
          | Where-Object { $_ -ne "" }
          | ForEach-Object { $_ -replace "\?.*$", "" }
        )
        Get-Help @Splat -Online | Out-Null
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
        Get-Help @Splat -Parameter $Parameter
      }
      catch {
        try {
          Get-Help @Splat
          Write-Warning "No offline help found for parameters '$Parameter' in topic '$Name'."
        }
        catch {
          throw "No offline help found for topic '$Name'."
        }
      }
    }
    else {
      try {
        Get-Help @Splat
      }
      catch {
        throw "No offline help found for topic '$Name'."
      }
    }
  }
  else {
    Get-Help -Name "Get-Help" -Online
    Get-Help Get-Help
    Write-Warning "No help topic specified, showing Get-Help by default."
  }

  if ($Articles.Count -gt 0) {
    Write-Output "`r`nOnline Help:"
    Write-Output $Articles
    | ForEach-Object { $_ -replace "^https?:\/\/", "http://" }
    | ForEach-Object { $_ -replace "^learn\.microsoft\.com\/en-us\/", "^learn.microsoft.com/" }
  }
  else {
    Write-Warning "No online help found."
  }
}
