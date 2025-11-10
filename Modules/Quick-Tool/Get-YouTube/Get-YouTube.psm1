New-Alias yt Get-YouTube
<#
.SYNOPSIS
Use yt-dlp to download YouTube videos.
.DESCRIPTION
This function is an alias for 'yt-dlp' and allows you to download YouTube videos or extract audio from them.
.LINK
https://github.com/yt-dlp/yt-dlp?tab=readme-ov-file#usage-and-options
#>
function Get-YouTube {
  param([string]$Video)

  if ($Video) {
    $Rest = $args
  }
  else {
    if ($args) {
      $Video = $args[0]
      $Rest = $args[1..$args.Count]
    }
    else {
      throw Write-Error "No video specified."
    }
  }

  $VideoUrl = ($Video.StartsWith('http://') -or $Video.StartsWith('https://')) ? $Video : ($Video -match '^(?:(?:www|m)\.)?youtube\.com/watch\?v=(?<video>[-\w]+).*$') ? "https://www.youtube.com/watch?v=$($Matches.video)" : "https://www.youtube.com/watch?v=$Video"

  if (Test-Url $VideoUrl) {
    & yt-dlp @Rest -- $VideoUrl
  }
  else {
    throw Write-Error "The specified YouTube video URL is not reachable: $VideoUrl"
  }
}

New-Alias yta Get-YouTubeAudio
<#
.SYNOPSIS
Use yt-dlp to extract audio from a YouTube video.
.DESCRIPTION
This function is an alias for 'yt-dlp' and extracts audio from a YouTube video.
.LINK
https://github.com/yt-dlp/yt-dlp?tab=readme-ov-file#video-format-options
#>
function Get-YouTubeAudio {
  param([string]$Video)

  Get-YouTube -Video $Video @args --format "bestaudio" --extract-audio --audio-format "mp3" --audio-quality 0 --audio-quality 0 --postprocessor-args "-ar 44100"
}

New-Alias ytf Get-YouTubeFormat
<#
.SYNOPSIS
Use yt-dlp to get available formats for a YouTube video.
.DESCRIPTION
This function is an alias for 'yt-dlp -F' and lists all available formats for a YouTube video.
.LINK
https://github.com/yt-dlp/yt-dlp?tab=readme-ov-file#video-format-options
#>
function Get-YouTubeFormat {
  param([string]$Video)

  Get-YouTube -Video $Video @args "-F"
}
