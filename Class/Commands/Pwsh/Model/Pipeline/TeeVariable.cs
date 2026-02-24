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
  public string Variable
  {
    get => variable;
    set => variable = value;
  }
  private string variable = string.Empty;

  [Parameter(
    ParameterSetName = "File",
    Mandatory = true
  )]
  [Alias("Path")]
  public string FilePath
  {
    get => path;
    set => path = value;
  }
  private string path = string.Empty;

  [Parameter(
    ParameterSetName = "LiteralFile",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  public string LiteralPath
  {
    get => literalPath;
    set => literalPath = value;
  }
  private string literalPath = string.Empty;

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
  [EnumCompletions(
    typeof(Client.FileSystem.Encoding)
  )]
  public string Encoding
  {
    get => encoding;
    set => encoding = int.TryParse(
      value,
      out _
    )
      ? encoding = value
      : System.Enum.TryParse(
          value,
          true,
          out Client.FileSystem.Encoding parsedEncoding
        )
          ? parsedEncoding.ToString()
          : encoding = value;
  }
  private string encoding = string.Empty;
}
