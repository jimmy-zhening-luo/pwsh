namespace Module.Shell.New.Directory
{
  namespace Commands
  {
    using System.IO;
    using System.Management.Automation;
    using Completer.PathCompleter;

    [Cmdlet(
      VerbsCommon.New,
      "Directory",
      DefaultParameterSetName = "pathSet",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096592"
    )]
    [Alias("mk")]
    [OutputType(typeof(DirectoryInfo))]
    public class NewDirectory : WrappedCommand
    {
      [Parameter(
        ParameterSetName = "pathSet",
        Mandatory = true,
        Position = 0,
        ValueFromPipelineByPropertyName = true
      )]
      [Parameter(
        ParameterSetName = "nameSet",
        Position = 0,
        ValueFromPipelineByPropertyName = true
      )]
      [RelativePathCompletions]
      public string[]? Path;

      [Parameter(
        ParameterSetName = "nameSet",
        Mandatory = true,
        ValueFromPipelineByPropertyName = true
      )]
      public string? Name;

      [Parameter(
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [Alias("Target")]
      [RelativePathCompletions]
      public object? Value;

      [Parameter]
      [Alias("f")]
      public SwitchParameter? Force;

      protected override string WrappedCommandName => "New-Item";

      protected override bool BeforeBeginProcessing()
      {
        BoundParameters()["ItemType"] = "Directory";

        return true;
      }
    }
  }
}
