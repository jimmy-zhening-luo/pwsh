namespace Module.Commands.Shell.Get.File;

public abstract class WrappedGetFile() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Get-Content"
)
{
  public abstract string[] Path { set; }
  private protected string[] paths = [];

  [Parameter]
  [SupportsWildcards]
  public required string Filter
  {
    private get;
    set;
  }

  [Parameter]
  [SupportsWildcards]
  public required string[] Include
  {
    private get;
    set;
  }

  [Parameter]
  [SupportsWildcards]
  public required string[] Exclude
  {
    private get;
    set;
  }

  [Parameter]
  public long ReadCount
  {
    private get;
    set;
  }

  [Parameter]
  [Alias("First", "Head")]
  [ValidateRange(
    0,
    9223372036854775807
  )]
  public long TotalCount
  {
    private get;
    set;
  }

  [Parameter]
  [Alias("Last")]
  [ValidateRange(0, int.MaxValue)]
  public int Tail
  {
    private get;
    set;
  }

  [Parameter]
  public required string Delimiter
  {
    private get;
    set;
  }

  [Parameter]
  public required string Stream
  {
    private get;
    set;
  }

  [Parameter]
  [ValidateNotNullOrEmpty]
  [Tab.EnumCompletions(typeof(Client.File.Encoding))]
  public required string Encoding
  {
    private get;
    set;
  }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    private get;
    set;
  }

  [Parameter]
  public SwitchParameter AsByteStream
  {
    private get;
    set;
  }

  [Parameter]
  public SwitchParameter Raw
  {
    private get;
    set;
  }

  [Parameter]
  public SwitchParameter Wait
  {
    private get;
    set;
  }

  private protected sealed override void TransformPipelineInput()
  {
    if (!InCurrentLocation)
    {
      BoundParameters["Path"] = paths = ReanchorPath(paths);
    }
  }
}
