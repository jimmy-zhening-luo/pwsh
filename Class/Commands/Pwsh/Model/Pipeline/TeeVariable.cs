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
  $@"{StandardModule.Utility}\Tee-Object"
)
{
  const string ParameterSetFile = "File";
  const string ParameterSetLiteralFile = "LiteralFile";

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
    ParameterSetName = nameof(Variable),
    Mandatory = true,
    Position = default
  )]
  required public string Variable
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = ParameterSetFile,
    Mandatory = true
  )]
  [Alias("Path")]
  required public string FilePath
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = ParameterSetLiteralFile,
    Mandatory = true
  )]
  [Alias(StandardAlias.PSPath, StandardAlias.LP)]
  required public string LiteralPath
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = ParameterSetFile
  )]
  public SwitchParameter Append
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = ParameterSetFile
  )]
  [Parameter(
    ParameterSetName = ParameterSetLiteralFile
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.EnumCompletions(typeof(Client.File.Encoding))]
  required public string Encoding
  {
    init => _ = value;
  }
}
