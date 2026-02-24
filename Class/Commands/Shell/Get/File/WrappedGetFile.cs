namespace Module.Commands.Shell.Get.File;

public abstract class WrappedGetFile() : WrappedCommand(
  "Get-Content"
)
{
  public abstract string[] Path
  {
    get;
    set;
  }
  private protected string[] paths = [];

  [Parameter(
    ParameterSetName = "Path",
    Position = 1
  )]
  [SupportsWildcards]
  public virtual string Filter { get; set; } = string.Empty;

  [Parameter]
  [SupportsWildcards]
  public string[] Include { get; set; } = [];

  [Parameter]
  [SupportsWildcards]
  public string[] Exclude { get; set; } = [];

  [Parameter]
  public long ReadCount { get; set; }

  [Parameter]
  [Alias("First", "Head")]
  [ValidateRange(0, 9223372036854775807)]
  public long TotalCount { get; set; }

  [Parameter]
  [Alias("Last")]
  [ValidateRange(0, 2147483647)]
  public int Tail { get; set; }

  [Parameter]
  public string Delimiter { get; set; } = string.Empty;

  [Parameter]
  public string Stream { get; set; } = string.Empty;

  [Parameter]
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

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    get => force;
    set => force = value;
  }
  private bool force;

  [Parameter]
  public SwitchParameter AsByteStream
  {
    get => asByteStream;
    set => asByteStream = value;
  }
  private bool asByteStream;

  [Parameter]
  public SwitchParameter Raw
  {
    get => raw;
    set => raw = value;
  }
  private bool raw;

  [Parameter]
  public SwitchParameter Wait
  {
    get => wait;
    set => wait = value;
  }
  private bool wait;

  private protected sealed override void TransformParameters()
  {
    if (!UsingCurrentLocation)
    {
      if (paths.Length == 0)
      {
        paths = [
          Reanchor()
        ];
      }
      else
      {
        for (
          int i = 0;
          i < paths.Length;
          i++
        )
        {
          paths[i] = Reanchor(
            paths[i]
          );
        }
      }

      MyInvocation.BoundParameters["Path"] = paths;
    }
  }
}
