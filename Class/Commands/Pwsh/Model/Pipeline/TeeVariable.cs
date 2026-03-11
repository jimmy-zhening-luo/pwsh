namespace PowerModule.Commands.Pwsh.Model.Pipeline;

[Cmdlet(
  "Tee",
  "Variable",
  DefaultParameterSetName = "Variable",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097034"
)]
[Alias("t")]
[OutputType(typeof(object))]
sealed public class TeeVariable() : WrappedCommand(
  @"Microsoft.PowerShell.Utility\Tee-Object",
  AcceptsPipelineInput: true
)
{
  sealed override private protected object PipelineInput => InputObject;

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
    init => Bind();
  }

  [Parameter(
    ParameterSetName = "File",
    Mandatory = true
  )]
  [Alias("Path")]
  required public string FilePath
  {
    init => Bind();
  }

  [Parameter(
    ParameterSetName = "LiteralFile",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  required public string LiteralPath
  {
    init => Bind();
  }

  [Parameter(
    ParameterSetName = "File"
  )]
  public SwitchParameter Append
  {
    init => Bind();
  }

  [Parameter(
    ParameterSetName = "File"
  )]
  [Parameter(
    ParameterSetName = "LiteralFile"
  )]
  [ValidateNotNullOrEmpty]
  [Tab.EnumCompletions(typeof(Client.File.Encoding))]
  required public string Encoding
  {
    init => Bind();
  }
}
