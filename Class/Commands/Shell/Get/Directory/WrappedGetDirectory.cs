namespace Module.Commands.Shell.Get.Directory;

public abstract class WrappedGetDirectory() : WrappedCommand(
  "Get-ChildItem"
)
{
  public abstract string[] Path { get; set; }
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
  private protected string filter = string.Empty;

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
  public FlagsExpression<System.IO.FileAttributes> Attributes { get; set; } = new FlagsExpression<System.IO.FileAttributes>(
    System.IO.FileAttributes.None.ToString()
  );

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
