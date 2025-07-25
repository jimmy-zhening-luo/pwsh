New-Alias yt yt-dlp

New-Alias yta Get-YouTubeAudio
function Get-YouTubeAudio {
  yt-dlp --format "bestaudio" --extract-audio --audio-format "mp3" --audio-quality 0 --audio-quality 0 --postprocessor-args "-ar 44100" @args
}
