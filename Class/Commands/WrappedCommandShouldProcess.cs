namespace Module.Commands;

public abstract class WrappedCommandShouldProcess(
  string WrappedCommandName,
  bool SkipSsh = false,
  string PipelineInputParameterName = "",
  CommandTypes CommandType = CommandTypes.Cmdlet
) : WrappedCommand(
  WrappedCommandName,
  SkipSsh,
  PipelineInputParameterName,
  CommandType
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
