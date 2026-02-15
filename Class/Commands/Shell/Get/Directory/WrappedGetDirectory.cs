namespace Module.Commands.Shell.Get.Directory;

public abstract class WrappedGetDirectory() : WrappedCommand(
  "Get-ChildItem"
)
{
  public abstract string[] Path
  {
    get;
    set;
  }
  private protected string[] paths = [];

  [Parameter(
    ParameterSetName = "Items",
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
  [Alias("s", "r")]
  public SwitchParameter Recurse
  {
    get => recurse;
    set => recurse = value;
  }
  private bool recurse;

  [Parameter]
  public uint Depth
  {
    get => depth;
    set => depth = value;
  }
  private uint depth;

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
  [Alias("ad")]
  public SwitchParameter Directory
  {
    get => directory;
    set => directory = value;
  }
  private bool directory;

  [Parameter]
  [Alias("af")]
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
  public FlagsExpression<IO.FileAttributes>? Attributes;

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

      BoundParameters["Path"] = paths;
    }
  }
}
