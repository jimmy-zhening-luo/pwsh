& {
  @(
    'clear'
    'rd'
    'man'
  ) |
    ForEach-Object {
      Remove-Alias $PSItem
    }

  @(
    'gm'
    'gp'
    'gu'
  ) |
    ForEach-Object {
      Remove-Alias $PSItem -Force
    }

  @(
    @{
      Name = 'verb'
      Value = 'Get-VerbList'
    }
  ) |
    ForEach-Object {
      New-Alias @PSItem -Option ReadOnly
    }
}
