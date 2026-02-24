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
  public virtual string Filter
  {
    get => filter;
    set => filter = value;
  }
  private protected string filter = "";

  [Parameter]
  [SupportsWildcards]
  public string[] Include
  {
    get => inclusions;
    set => inclusions = value;
  }
  private string[] inclusions = [];

  [Parameter]
  [SupportsWildcards]
  public string[] Exclude
  {
    get => exclusions;
    set => exclusions = value;
  }
  private string[] exclusions = [];

  [Parameter]
  public long ReadCount
  {
    get => readCount;
    set => readCount = value;
  }
  private long readCount;

  [Parameter]
  [Alias("First", "Head")]
  [ValidateRange(0, 9223372036854775807)]
  public long TotalCount
  {
    get => totalCount;
    set => totalCount = value;
  }
  private long totalCount;

  [Parameter]
  [Alias("Last")]
  [ValidateRange(0, 2147483647)]
  public int Tail
  {
    get => tail;
    set => tail = value;
  }
  private int tail;

  [Parameter]
  public string Delimiter
  {
    get => delimiter;
    set => delimiter = value;
  }
  private string delimiter = "";

  [Parameter]
  public string Stream
  {
    get => stream;
    set => stream = value;
  }
  private string stream = "";

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
  private string encoding = "";

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
