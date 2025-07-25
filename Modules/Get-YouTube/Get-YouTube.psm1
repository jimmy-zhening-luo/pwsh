New-Alias yt Get-YouTube
function Get-YouTube {
  param(
    [System.String]$Video
  )

  if (-not $Video) {
    if ($args) {
      $Video = $args[0]
      $Rest = $args[1..$args.Length]
    }
    else {
      throw Write-Error "No video specified."
    }
  }
  else {
    $Rest = $args
  }

  $VideoUrl = ($Video.StartsWith('http://') -or $Video.StartsWith('https://')) ? $Video : ($Video -match '^(?:(?:www|m)\.)?youtube\.com/watch\?v=(?<video>[-\w]+).*$') ? "https://www.youtube.com/watch?v=$($Matches.video)" : "https://www.youtube.com/watch?v=$Video"

  yt-dlp @Rest -- $VideoUrl
}

New-Alias yta Get-YouTubeAudio
function Get-YouTubeAudio {
  param(
    [System.String]$Video
  )

  Get-YouTube -Video $Video @args --format "bestaudio" --extract-audio --audio-format "mp3" --audio-quality 0 --audio-quality 0 --postprocessor-args "-ar 44100"
}

New-Alias ytf Get-YouTubeFormat
function Get-YouTubeFormat {
  param(
    [System.String]$Video
  )

  Get-YouTube -Video $Video @args "-F"
}
