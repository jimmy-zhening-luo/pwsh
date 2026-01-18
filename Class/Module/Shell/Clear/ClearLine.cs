namespace Module.Shell.Clear
{
  namespace Commands
  {
    using System;
    using System.Management.Automation;
    using Completer.PathCompleter;

    [Cmdlet(
      VerbsCommon.Clear,
      "Line",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096807"
    )]
    [Alias("cl", "clear")]
    [OutputType(typeof(void))]
    public class ClearLine : WrappedCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0
      )]
      [AllowEmptyString]
      [SupportsWildcards]
      [RelativePathCompletions]
      public string Path;

      [Parameter(
        Position = 1
      )]
      [SupportsWildcards]
      public string Filter;

      [Parameter(
        ParameterSetName = "LiteralPath",
        Mandatory = true,
        ValueFromPipelineByPropertyName = true
      )]
      [Alias("PSPath", "LP")]
      public string[] LiteralPath;

      [Parameter]
      [SupportsWildcards]
      public string[] Include;

      [Parameter]
      [SupportsWildcards]
      public string[] Exclude;

      [Parameter]
      [Alias("f")]
      public SwitchParameter Force;

      [Parameter]
      public string Stream;

      protected override string WrappedCommandName() => "Clear-Content";

      protected override bool BeforeBeginProcessing() => Path != null
        || ParameterSetName == "LiteralPath";

      protected override void BeforeEndProcessing()
      {
        if (
          !IsPresent("Path")
          && !IsPresent("LiteralPath")
        )
        {
          Console.Clear();
        }
      }
    }
  }
}
