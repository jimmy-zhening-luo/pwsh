namespace Module.Commands;

public abstract class WrappedCommandShouldProcess(
  string WrappedCommandName,
  string? PipelineInputParameterName = default,
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
    private protected get;
    set;
  }

  [Parameter]
  [Alias("cf")]
  public SwitchParameter Confirm
  {
    private protected get;
    set;
  }
}
