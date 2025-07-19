New-Alias -Name run -Value Invoke-CommandPrompt -Option ReadOnly
function Invoke-CommandPrompt {
  cmd /c @args
}
