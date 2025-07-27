New-Alias yt Get-YouTube
function Get-YouTube {
  param([System.String]$Video)

  if ($Video) {
    $Rest = $args
  }
  else {
    if ($args) {
      $Video = $args[0]
      $Rest = $args[1..$args.Length]
    }
    else {
      throw Write-Error "No video specified."
    }
  }

  $VideoUrl = ($Video.StartsWith('http://') -or $Video.StartsWith('https://')) ? $Video : ($Video -match '^(?:(?:www|m)\.)?youtube\.com/watch\?v=(?<video>[-\w]+).*$') ? "https://www.youtube.com/watch?v=$($Matches.video)" : "https://www.youtube.com/watch?v=$Video"

  if (Test-Url $VideoUrl) {
    yt-dlp @Rest -- $VideoUrl
  }
  else {
    throw Write-Error "The specified YouTube video URL is not reachable: $VideoUrl"
  }
}

New-Alias yta Get-YouTubeAudio
function Get-YouTubeAudio {
  param([System.String]$Video)

  Get-YouTube -Video $Video @args --format "bestaudio" --extract-audio --audio-format "mp3" --audio-quality 0 --audio-quality 0 --postprocessor-args "-ar 44100"
}

New-Alias ytf Get-YouTubeFormat
function Get-YouTubeFormat {
  param([System.String]$Video)

  Get-YouTube -Video $Video @args "-F"
}
