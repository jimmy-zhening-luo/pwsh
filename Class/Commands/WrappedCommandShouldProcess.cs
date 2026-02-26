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
  public required SwitchParameter WhatIf { get; set; }

  [Parameter]
  [Alias("cf")]
  public required SwitchParameter Confirm { get; set; }
}
