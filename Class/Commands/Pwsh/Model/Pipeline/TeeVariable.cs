namespace Module.Commands.Pwsh.Model.Pipeline;

[Cmdlet(
  "Tee",
  "Variable",
  DefaultParameterSetName = "Variable",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097034"
)]
[Alias("t")]
[OutputType(typeof(object))]
public sealed class TeeVariable() : WrappedCommand(
  @"Microsoft.PowerShell.Utility\Tee-Object",
  default,
  "InputObject"
)
{
  [Parameter(
    ValueFromPipeline = true
  )]
  public required object InputObject;

  [Parameter(
    ParameterSetName = "Variable",
    Mandatory = true,
    Position = default
  )]
  public string Variable { get; set; } = string.Empty;

  [Parameter(
    ParameterSetName = "File",
    Mandatory = true
  )]
  [Alias("Path")]
  public string FilePath { get; set; } = string.Empty;

  [Parameter(
    ParameterSetName = "LiteralFile",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  public string LiteralPath { get; set; } = string.Empty;

  [Parameter(
    ParameterSetName = "File"
  )]
  public SwitchParameter Append
  {
    get => append;
    set => append = value;
  }
  private bool append;

  [Parameter(
    ParameterSetName = "File"
  )]
  [Parameter(
    ParameterSetName = "LiteralFile"
  )]
  [ValidateNotNullOrEmpty]
  [EnumCompletions(
    typeof(Client.FileSystem.Encoding)
  )]
  public string Encoding { get; set; } = string.Empty;
}
