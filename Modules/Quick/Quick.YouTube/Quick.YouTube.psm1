New-Alias yt Quick\Get-YouTube
<#
.SYNOPSIS
Use yt-dlp to download YouTube videos.
.DESCRIPTION
This function is an alias for 'yt-dlp' and allows you to download YouTube videos or extract audio from them.
.LINK
https://github.com/yt-dlp/yt-dlp?tab=readme-ov-file#usage-and-options
#>
function Get-YouTube {
  param(
    [Parameter(Mandatory)]
    [string]$Video
  )

  $VideoUri = $Video -match '^(?>https?://)?(?>(?>www|m)\.)?(?>youtube\.com/watch\?)\S*v=(?<Video>[-\w]+)' ? [UriBuilder]::new(
    'https',
    'www.youtube.com',
    -1,
    '/watch',
    '?v=' + $Matches.Video
  ).Uri : [Uri]$Video

  if (Browse\Test-Url -Uri $VideoUri) {
    & yt-dlp @args -- $VideoUri.OriginalString
  }
  else {
    throw 'The specified video URL is unreachable: ' + $VideoUri.OriginalString
  }
}

New-Alias yta Quick\Get-YouTubeAudio
<#
.SYNOPSIS
Use yt-dlp to extract audio from a YouTube video.
.DESCRIPTION
This function is an alias for 'yt-dlp' and extracts audio from a YouTube video.
.LINK
https://github.com/yt-dlp/yt-dlp?tab=readme-ov-file#video-format-options
#>
function Get-YouTubeAudio {
  param(
    [string]$Video
  )

  $YtArguments = @(
    '--format'
    'bestaudio'
    '--extract-audio'
    '--audio-format'
    'mp3'
    '--audio-quality'
    0
    '--postprocessor-args'
    '-ar 44100'
  )
  Get-YouTube @PSBoundParameters @args @YtArguments
}

New-Alias ytf Quick\Get-YouTubeFormat
<#
.SYNOPSIS
Use yt-dlp to get available formats for a YouTube video.
.DESCRIPTION
This function is an alias for 'yt-dlp -F' and lists all available formats for a YouTube video.
.LINK
https://github.com/yt-dlp/yt-dlp?tab=readme-ov-file#video-format-options
#>
function Get-YouTubeFormat {
  param(
    [string]$Video
  )

  $YtArguments = @(
    '-F'
  )
  Get-YouTube @PSBoundParameters @args @YTArguments
}

New-Alias yte Quick\Invoke-YouTubeDirectory
function Invoke-YouTubeDirectory {
  $YouTubeDownloads = @{
    Path = 'Videos\YouTube'
  }
  Shell\Invoke-DirectoryHome @YouTubeDownloads
}

New-Alias ytc Quick\Invoke-YouTubeConfig
function Invoke-YouTubeConfig {
  $YouTubeConfig = @{
    Path        = 'util\bin\yt\yt-dlp.conf'
    ProfileName = 'Setting'
    Window      = $True
  }
  Shell\Invoke-WorkspaceHome @YouTubeConfig @args
}
