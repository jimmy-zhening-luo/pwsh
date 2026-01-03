<#
.SYNOPSIS
Use yt-dlp to download YouTube videos.

.DESCRIPTION
This function is an alias for 'yt-dlp' and allows you to download YouTube videos or extract audio from them.

.COMPONENT
Cli.YouTube

.LINK
https://github.com/yt-dlp/yt-dlp?tab=readme-ov-file#usage-and-options
#>
function Get-YouTube {

  [Alias('yt')]
  param(

    # The YouTube video URL or identifier to process.
    [string]$Video
  )

  if (-not $Video) {
    throw 'No video specified.'
  }

  [uri]$VideoUri = $Video -match '^(?>https?://)?(?>(?>www|m)\.)?(?>youtube\.com/watch\?)(?:\S*&)?v=(?<Video>(?>[-\w]+))' ? [UriBuilder]::new(
    'https',
    'www.youtube.com',
    -1,
    '/watch',
    '?v=' + $Matches.Video
  ).Uri : $Video

  if (Test-Url -Uri $VideoUri) {
    & yt-dlp.exe @args -- [string]$VideoUri

    if ($LASTEXITCODE -notin 0, 1) {
      throw "ytdlp error, execution stopped with exit code: $LASTEXITCODE"
    }
  }
  else {
    throw 'The specified video URL is unreachable: ' + [string]$VideoUri
  }
}

<#
.SYNOPSIS
Use yt-dlp to extract audio from a YouTube video.

.DESCRIPTION
This function is an alias for 'yt-dlp' and extracts audio from a YouTube video.

.COMPONENT
Cli.YouTube

.LINK
https://github.com/yt-dlp/yt-dlp?tab=readme-ov-file#post-processing-options
#>
function Get-YouTubeAudio {

  [Alias('yta')]
  param(

    # The YouTube video URL or identifier to process.
    [string]$Video
  )

  $YouTubeArgument = @(
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
  Get-YouTube -Video $Video @args @YouTubeArgument
}

<#
.SYNOPSIS
Use yt-dlp to get available formats for a YouTube video.

.DESCRIPTION
This function is an alias for 'yt-dlp -F' and lists all available formats for a YouTube video.

.COMPONENT
Cli.YouTube

.LINK
https://github.com/yt-dlp/yt-dlp?tab=readme-ov-file#video-format-options
#>
function Get-YouTubeFormat {

  [Alias('ytf')]
  param(

    # The YouTube video URL or identifier to process.
    [string]$Video
  )

  Get-YouTube -Video $Video @args '-F'
}

<#
.SYNOPSIS
Opens yt-dlp configuration file for editing.

.DESCRIPTION
Opens yt-dlp configuration file for editing.

.COMPONENT
Cli.YouTube
#>
function Invoke-YouTubeConfig {
  [CmdletBinding()]
  [OutputType([void])]
  [Alias('ytc')]
  param()

  Start-WorkspaceHome -Workspace util\bin\yt\yt-dlp.conf -ProfileName Setting
}
