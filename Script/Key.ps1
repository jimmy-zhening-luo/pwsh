& {
  Import-Module PSReadLine

  @(
    @{
      Chord = 'Shift+DownArrow'
      Function = 'NextHistory'
    }  
    @{
      Chord = 'Shift+UpArrow'
      Function = 'PreviousHistory'
    }
    @{
      Chord = 'Shift+UpArrow'
      Function = 'PreviousHistory'
    }
    @{
      Chord = 'Shift+UpArrow'
      Function = 'PreviousHistory'
    }
    @{
      Chord = 'Shift+UpArrow'
      Function = 'PreviousHistory'
    }
    @{
      Chord = 'Shift+UpArrow'
      Function = 'PreviousHistory'
    }
  ) |
    ForEach-Object {
      Set-PSReadLineKeyHandler @PSItem
    }
}
