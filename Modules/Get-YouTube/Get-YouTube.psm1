New-Alias yt yt-dlp

New-Alias yta Get-YouTubeAudio
function Get-YouTubeAudio {
  yt-dlp -f "140/bestaudio,bestaudio" @args
}
