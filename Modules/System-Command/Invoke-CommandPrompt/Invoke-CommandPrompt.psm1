New-Alias -Name run -Value Invoke-CommandPrompt
function Invoke-CommandPrompt {
  cmd /c @args
}
