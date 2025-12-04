using namespace System.Net;

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
        % { [WebUtility]::UrlEncode($_) }
    )
  }

  $Tokens -join '+'
}

New-Alias search Browse\Search-Query
New-Alias g Browse\Search-Query

function Search-Query {
  param(
    [string[]]$Terms
  )

  $PSBoundParameters.Terms += $args
  $QueryString = Expand-Query @PSBoundParameters
  $QueryUri = [UriBuilder]::new(
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
    Browse\Open-Url -Uri $QueryUri
  }
}

New-Alias maps Browse\Search-Map
New-Alias map Browse\Search-Map

function Search-Map {
  param(
    [string[]]$Terms
  )

  $PSBoundParameters.Terms += $args
  $QueryString = Expand-Query @PSBoundParameters
  $QueryUri = [UriBuilder]::new(
    'https',
    'www.google.com',
    -1,
    "/maps/search/$QueryString/"
  ).Uri

  if ($env:SSH_CLIENT) {
    $QueryUri.AbsoluteUri
  }
  else {
    Browse\Open-Url -Uri $QueryUri
  }
}
