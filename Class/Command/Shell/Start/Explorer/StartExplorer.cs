namespace Module.Shell.Start.Explorer
{
  namespace Commands
  {
    using System.Management.Automation;

    [Cmdlet(
      VerbsLifecycle.Start,
      "Explorer",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
    )]
    [Alias("e")]
    [OutputType(typeof(void))]
    public class StartExplorer : WrappedCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyCollection]
      [SupportsWildcards]
      [RelativePathCompletions]
      public string[]? Path;

      [Parameter(
        ParameterSetName = "LiteralPath",
        Mandatory = true,
        ValueFromPipelineByPropertyName = true
      )]
      [Alias("PSPath", "LP")]
      public string[]? LiteralPath;

      [Parameter]
      [SupportsWildcards]
      public string? Filter;

      [Parameter]
      [SupportsWildcards]
      public string[]? Include;

      [Parameter]
      [SupportsWildcards]
      public string[]? Exclude;

      protected override string WrappedCommandName => "Invoke-Item";

      protected override bool NoSsh => true;

      protected override bool BeforeBeginProcessing()
      {
        if (!IsPresent("Path"))
        {
          BoundParameters["Path"] = new string[] { Pwd() };
        }

        return true;
      }
    }
  }
}
