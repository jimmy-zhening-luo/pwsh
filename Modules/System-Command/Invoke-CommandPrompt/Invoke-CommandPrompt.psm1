New-Alias -Option ReadOnly -Name run -Value Invoke-CommandPrompt
function Invoke-CommandPrompt {
  cmd /c @args
}
