namespace Module.Command.Shell.Get.File;

public abstract class WrappedGetFile : WrappedCommand
{
  private protected WrappedGetFile() : base(
    "Get-Content"
  )
  { }

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
  public System.Text.Encoding Encoding
  {
    get => encoding;
    set => encoding = value;
  }
  private System.Text.Encoding encoding = new System.Text.UTF8Encoding(false, true);

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force;

  [Parameter]
  public SwitchParameter AsByteStream;

  [Parameter]
  public SwitchParameter Raw;

  [Parameter]
  public SwitchParameter Wait;

  private protected sealed override void TransformParameters()
  {
    if (!UsingCurrentLocation)
    {
      if (paths.Length != 0)
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
      else
      {
        paths = new string[]
        {
          Reanchor()
        };
      }

      BoundParameters["Path"] = paths;
    }
  }
}
