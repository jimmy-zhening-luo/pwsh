namespace Module.Command.Shell.Get.Directory;

public abstract class WrappedGetDirectory : WrappedCommand
{
  private protected WrappedGetDirectory() : base(
    "Get-ChildItem"
  )
  { }

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
  public SwitchParameter Recurse;

  [Parameter]
  public uint Depth
  {
    get => depth;
    set => depth = value;
  }
  private uint depth;

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force;

  [Parameter]
  public SwitchParameter Name;

  [Parameter]
  [Alias("ad")]
  public SwitchParameter Directory;

  [Parameter]
  [Alias("af")]
  public SwitchParameter File;

  [Parameter]
  [Alias("ah", "h")]
  public SwitchParameter Hidden;

  [Parameter]
  [Alias("as")]
  public SwitchParameter System;

  [Parameter]
  [Alias("ar")]
  public SwitchParameter ReadOnly;

  [Parameter]
  public SwitchParameter FollowSymlink;

  [Parameter]
  public FlagsExpression<IO.FileAttributes>? Attributes;

  private protected sealed override void TransformParameters()
  {
    if (!UsingCurrentLocation)
    {
      if (IsPresent("Path"))
      {
        string[] paths = (string[])BoundParameters["Path"];

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

        BoundParameters["Path"] = paths;
      }
      else
      {
        BoundParameters["Path"] = new string[]
        {
          Reanchor()
        };
      }
    }
  }
}
