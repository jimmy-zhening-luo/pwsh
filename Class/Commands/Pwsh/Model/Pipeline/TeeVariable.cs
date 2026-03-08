namespace Module.Commands.Pwsh.Model.Pipeline;

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
  sealed private protected override object? PipelineInput => InputObject;

  [Parameter(
    ValueFromPipeline = true
  )]
  public required object InputObject { get; set; }

  [Parameter(
    ParameterSetName = "Variable",
    Mandatory = true,
    Position = default
  )]
  public required string Variable
  {
    private get;
    set;
  }

  [Parameter(
    ParameterSetName = "File",
    Mandatory = true
  )]
  [Alias("Path")]
  public required string FilePath
  {
    private get;
    set;
  }

  [Parameter(
    ParameterSetName = "LiteralFile",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  public required string LiteralPath
  {
    private get;
    set;
  }

  [Parameter(
    ParameterSetName = "File"
  )]
  public SwitchParameter Append
  {
    private get;
    set;
  }

  [Parameter(
    ParameterSetName = "File"
  )]
  [Parameter(
    ParameterSetName = "LiteralFile"
  )]
  [ValidateNotNullOrEmpty]
  [Tab.EnumCompletions(typeof(Client.File.Encoding))]
  public required string Encoding
  {
    private get;
    set;
  }
}
