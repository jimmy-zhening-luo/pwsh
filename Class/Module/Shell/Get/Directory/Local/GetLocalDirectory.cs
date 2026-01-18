namespace Module.Shell.Get.Directory.Local
{
  using System.IO;
  using System.Management.Automation;

  public abstract class GetLocalDirectoryCommand : LocalWrappedCommand
  {
    [Parameter(
      ParameterSetName = "Items",
      Position = 0,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true
    )]
    [AllowEmptyCollection]
    [SupportsWildcards]
    public string[] Path;

    [Parameter(
      ParameterSetName = "Items",
      Position = 1
    )]
    [SupportsWildcards]
    public string Filter;

    [Parameter]
    [SupportsWildcards]
    public string[] Include;

    [Parameter]
    [SupportsWildcards]
    public string[] Exclude;

    [Parameter]
    [Alias("s", "r")]
    public SwitchParameter Recurse;

    [Parameter]
    public uint Depth;

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
    public FlagsExpression<FileAttributes> Attributes;

    protected override string WrappedCommandName() => "Get-ChildItem";

    protected override bool BeforeBeginProcessing()
    {
      if (IsPresent("Path"))
      {
        string[] paths = (string[])BoundParameters()["Path"];

        for (int i = 0; i < paths.Length; i++)
        {
          paths[i] = Reanchor(paths[i]);
        }

        BoundParameters()["Path"] = paths;
      }
      else
      {
        BoundParameters()["Path"] = new string[] { Reanchor(string.Empty) };
      }
      return true;
    }
  }
}
