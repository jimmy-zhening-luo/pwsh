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
  "Tee-Object",
  false,
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
    Position = 0
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
  public string Encoding
  {
    get => encoding;
    set
    {
      encoding = int.TryParse(
        value,
        out var parsedInt
      )
        ? parsedInt.ToString()
        : System.Enum.TryParse<Client.FileSystem.Encoding>(
            value,
            true,
            out var parsedEnum
          )
            ? parsedEnum.ToString()
            : value;

      if (
        string.IsNullOrEmpty(
          encoding
        )
      )
      {
        MyInvocation.BoundParameters.Remove(
          "Encoding"
        );
      }
      else
      {
        MyInvocation.BoundParameters["Encoding"] = encoding;
      }
    }
  }
  private string encoding = string.Empty;
}
