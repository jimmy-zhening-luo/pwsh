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
  "InputObject"
)
{
  [Parameter(
    ValueFromPipeline = true
  )]
  public required object InputObject { get; set; }

  [Parameter(
    ParameterSetName = "Variable",
    Mandatory = true,
    Position = default
  )]
  public required string Variable { get; set; }

  [Parameter(
    ParameterSetName = "File",
    Mandatory = true
  )]
  [Alias("Path")]
  public required string FilePath { get; set; }

  [Parameter(
    ParameterSetName = "LiteralFile",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  public required string LiteralPath { get; set; }

  [Parameter(
    ParameterSetName = "File"
  )]
  public SwitchParameter Append { get; set; }

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
  public required string Encoding { get; set; }
}
