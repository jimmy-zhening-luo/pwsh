New-Alias yt yt-dlp

New-Alias yta Get-YouTubeAudio
function Get-YouTubeAudio {
  yt-dlp --extract-audio --format "140/bestaudio,bestaudio" @args
}
