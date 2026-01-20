namespace Module.Shell.Set.Directory
{
  namespace Commands
  {
    using System.Management.Automation;
    using Module.Completer.PathCompleter;

    [Cmdlet(
      VerbsCommon.Set,
      "Directory",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("c")]
    [OutputType(
      typeof(PathInfo),
      typeof(PathInfoStack)
    )]
    public class SetDirectory : WrappedCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyString]
      [SupportsWildcards]
      [RelativePathCompletions(
        "",
        PathItemType.Directory
      )]
      public string? Path;

      [Parameter(
        ParameterSetName = "LiteralPath",
        Mandatory = true,
        ValueFromPipelineByPropertyName = true
      )]
      [Alias("PSPath", "LP")]
      public string? LiteralPath;

      [Parameter(
        ParameterSetName = "Stack",
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyString]
      public string? Stack;

      [Parameter]
      public SwitchParameter? PassThru;

      protected override string WrappedCommandName => "Set-Location";

      protected override bool BeforeBeginProcessing()
      {
        if (Path == null && LiteralPath == null)
        {
          string pwd = Pwd();
          string parent = System.IO.Path.GetFullPath(
            "..",
            pwd
          );

          BoundParameters["Path"] = parent == pwd
            ? Home()
            : parent;
        }

        return true;
      }
    }
  }
}
