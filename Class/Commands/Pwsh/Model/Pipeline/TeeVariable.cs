namespace PowerModule.Commands.Pwsh.Model.Pipeline;

[Cmdlet(
  "Tee",
  "Variable",
  DefaultParameterSetName = "Variable",
  HelpUri = $"{HelpLink}2097034"
)]
[Alias("t")]
[OutputType(typeof(object))]
sealed public class TeeVariable() : WrappedCommand(
  @"Microsoft.PowerShell.Utility\Tee-Object"
)
{
  sealed override private protected PipelineInputSource PipelineInput => () => (
    "InputObject",
    InputObject
  );

  [Parameter(
    ValueFromPipeline = true
  )]
  required public object InputObject
  { get; init; }

  [Parameter(
    ParameterSetName = "Variable",
    Mandatory = true,
    Position = default
  )]
  required public string Variable
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = "File",
    Mandatory = true
  )]
  [Alias(StandardParameter.Path)]
  required public string FilePath
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = "LiteralFile",
    Mandatory = true
  )]
  [Alias(StandardAlias.PSPath, StandardAlias.LP)]
  required public string LiteralPath
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = "File"
  )]
  public SwitchParameter Append
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = "File"
  )]
  [Parameter(
    ParameterSetName = "LiteralFile"
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.EnumCompletions(typeof(Client.File.Encoding))]
  required public string Encoding
  {
    init => _ = value;
  }
}
