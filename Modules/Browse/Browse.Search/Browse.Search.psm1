function Expand-Query {
  param(
    [string[]]$Terms
  )

  $Terms += $args
  $Tokens = @()

  foreach ($Term in $Terms) {
    $Tokens += (
      $Term -split '\s+' |
        ? { $_ } |
        % { [System.Net.WebUtility]::UrlEncode($_) }
    )
  }

  $Tokens -join '+'
}

New-Alias search Search-Query
New-Alias g Search-Query

function Search-Query {
  param(
    [string[]]$Terms
  )

  $PSBoundParameters.Terms += $args
  $QueryString = Expand-Query @PSBoundParameters
  $QueryUri = [System.UriBuilder]::new(
    'https',
    'www.google.com',
    -1,
    '/search',
    "?q=$QueryString"
  ).Uri

  if ($env:SSH_CLIENT) {
    $QueryUri.AbsoluteUri
  }
  else {
    Open-Url -Uri $QueryUri
  }
}

New-Alias maps Search-Map
New-Alias map Search-Map

function Search-Map {
  param(
    [string[]]$Terms
  )

  $PSBoundParameters.Terms += $args
  $QueryString = Expand-Query @PSBoundParameters
  $QueryUri = [System.UriBuilder]::new(
    'https',
    'www.google.com',
    -1,
    "/maps/search/$QueryString/"
  ).Uri

  if ($env:SSH_CLIENT) {
    $QueryUri.AbsoluteUri
  }
  else {
    Open-Url -Uri $QueryUri
  }
}
