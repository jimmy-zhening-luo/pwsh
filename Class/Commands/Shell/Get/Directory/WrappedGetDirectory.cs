namespace Module.Commands.Shell.Get.Directory;

public abstract class WrappedGetDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Get-ChildItem"
)
{
  public abstract string[] Path { get; set; }
  private protected string[] paths = [];

  [Parameter(
    Position = 1
  )]
  [SupportsWildcards]
  [ValidateNotNullOrEmpty]
  public string Filter
  {
    get => filter;
    set
    {
      filter = value.Contains(
        '*'
      )
        ? value
        : $"{value}*";
    }
  }
  private string filter = string.Empty;

  [Parameter]
  [SupportsWildcards]
  public string[] Include { get; set; } = [];

  [Parameter]
  [SupportsWildcards]
  public string[] Exclude { get; set; } = [];

  [Parameter]
  [Alias("s", "r")]
  public SwitchParameter Recurse
  {
    get => recurse;
    set => recurse = value;
  }
  private bool recurse;

  [Parameter]
  [Alias("de")]
  public uint Depth { get; set; }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    get => force;
    set => force = value;
  }
  private bool force;

  [Parameter]
  public SwitchParameter Name
  {
    get => name;
    set => name = value;
  }
  private bool name;

  [Parameter]
  [Alias("ad", "d")]
  public SwitchParameter Directory
  {
    get => directory;
    set => directory = value;
  }
  private bool directory;

  [Parameter]
  [Alias("af", "fi")]
  public SwitchParameter File
  {
    get => file;
    set => file = value;
  }
  private bool file;

  [Parameter]
  [Alias("ah", "h")]
  public SwitchParameter Hidden
  {
    get => hidden;
    set => hidden = value;
  }
  private bool hidden;

  [Parameter]
  [Alias("as")]
  public SwitchParameter System
  {
    get => system;
    set => system = value;
  }
  private bool system;

  [Parameter]
  [Alias("ar")]
  public SwitchParameter ReadOnly
  {
    get => readOnly;
    set => readOnly = value;
  }
  private bool readOnly;

  [Parameter]
  public SwitchParameter FollowSymlink
  {
    get => followSymlink;
    set => followSymlink = value;
  }
  private bool followSymlink;

  [Parameter]
  [EnumCompletions(
    typeof(System.IO.FileAttributes)
  )]
  public FlagsExpression<System.IO.FileAttributes> Attributes { get; set; } = new(
    nameof(
      global::System.IO.FileAttributes.None
    )
  );

  private protected sealed override void TransformArguments()
  {
    if (Filter is not "")
    {
      BoundParameters["Filter"] = Filter;
    }
  }

  private protected sealed override void TransformPipelineInput()
  {
    if (!UsingCurrentLocation)
    {
      if (Path is [])
      {
        Path = [
          Reanchor()
        ];
      }
      else
      {
        for (
          int i = default;
          i < Path.Length;
          ++i
        )
        {
          Path[i] = Reanchor(
            Path[i]
          );
        }
      }

      BoundParameters["Path"] = Path;
    }
  }
}
