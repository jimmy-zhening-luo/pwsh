namespace Module.Command;

public abstract class WrappedCommandShouldProcess(
  string WrappedCommandName
) : WrappedCommand(WrappedCommandName)
{
  [Parameter]
  [Alias("wi")]
  public SwitchParameter WhatIf;

  [Parameter]
  [Alias("cf")]
  public SwitchParameter Confirm;
}
