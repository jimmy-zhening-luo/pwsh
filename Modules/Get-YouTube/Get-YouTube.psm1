New-Alias yt Get-YouTube
function Get-YouTube {
  param(
    [System.String]$Video
  )

  if (-not $Video) {
    Write-Error "No video specified."
    return
  }

  $VideoUrl = (Test-Url $Video) ? (
    ($Video.StartsWith('http://') -or $Video.StartsWith('https://')) ? $Video : "https://$Video"
  ) : "https://www.youtube.com/watch?v=$Video"

  yt-dlp @args -- $VideoUrl
}

New-Alias yta Get-YouTubeAudio
function Get-YouTubeAudio {
  param(
    [System.String]$Video
  )

  Get-YouTube -Video $Video --format "bestaudio" --extract-audio --audio-format "mp3" --audio-quality 0 --audio-quality 0 --postprocessor-args "-ar 44100" @args
}

New-Alias ytf Get-YouTubeFormat
function Get-YouTubeFormat {
  param(
    [System.String]$Video
  )

  Get-YouTube -Video $Video "-F" @args
}
