namespace Module.Commands;

public abstract class WrappedCommandShouldProcess(
  string WrappedCommandName,
  string PipelineInputParameterName = "",
  CommandTypes CommandType = CommandTypes.Cmdlet,
  bool SkipSsh = default
) : WrappedCommand(
  WrappedCommandName,
  PipelineInputParameterName,
  CommandType,
  SkipSsh
)
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
