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

  param(

    # The YouTube video URL or identifier to process.
    [string]$Video

  )

  if (-not $Video) {
    throw 'No video specified.'
  }

  [uri]$Private:VideoUri = $Video -match [regex]'^(?>https?://)?(?>(?>www|m)\.)?(?>youtube\.com/watch\?)(?:\S*&)?v=(?<Video>(?>[-\w]+))' ? [UriBuilder]::new(
    'https',
    'www.youtube.com',
    -1,
    '/watch',
    '?v=' + $Matches.Video
  ).Uri : [uri]$Video

  if (Test-Url -Uri $Private:VideoUri) {
    & yt-dlp.exe @args -- [string]$Private:VideoUri

    if ($LASTEXITCODE -ne 0) {
      throw "ytdlp error, execution stopped with exit code: $LASTEXITCODE"
    }
  }
  else {
    throw 'The specified video URL is unreachable: ' + [string]$Private:VideoUri
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

  param(

    # The YouTube video URL or identifier to process.
    [string]$Video

  )

  [string[]]$Private:YouTubeArgument = @(
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
  Get-YouTube @PSBoundParameters @args @Private:YouTubeArgument
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

  param(

    # The YouTube video URL or identifier to process.
    [string]$Video

  )

  [string[]]$Private:YouTubeArgument = @(
    '-F'
  )
  Get-YouTube @PSBoundParameters @args @Private:YouTubeArgument
}

<#
.SYNOPSIS
Opens yt-dlp download location in File Explorer.

.DESCRIPTION
Opens yt-dlp download location in File Explorer, not by config though lmao.

.COMPONENT
Cli.YouTube
#>
function Invoke-YouTubeDirectory {

  [CmdletBinding()]
  [OutputType([void])]

  param()

  [hashtable]$Private:YouTubeDownloads = @{
    Path = 'Videos\YouTube'
  }
  Invoke-DirectoryHome @Private:YouTubeDownloads
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

  param()

  [hashtable]$Private:YouTubeConfig = @{
    Workspace   = 'util\bin\yt\yt-dlp.conf'
    ProfileName = 'Setting'
    Window      = $True
  }
  Invoke-WorkspaceHome @Private:YouTubeConfig
}

New-Alias yt Get-YouTube
New-Alias yta Get-YouTubeAudio
New-Alias ytf Get-YouTubeFormat
New-Alias yte Invoke-YouTubeDirectory
New-Alias ytc Invoke-YouTubeConfig
