namespace Module.Command;

public abstract class WrappedCommandShouldProcess(
  string WrappedCommandName
) : WrappedCommand(WrappedCommandName)
{
  [Parameter]
  [Alias("wi")]
  public SwitchParameter WhatIf
  {
    get => whatif;
    set => whatif = value;
  }
  private bool whatif;

  [Parameter]
  [Alias("cf")]
  public SwitchParameter Confirm
  {
    get => confirm;
    set => confirm = value;
  }
  private bool confirm;
}
