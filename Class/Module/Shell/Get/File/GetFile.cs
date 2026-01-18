namespace Module.Shell.Get.File
{
  namespace Commands
  {
    using System.IO;
    using System.Text;
    using System.Management.Automation;
    using Completer.PathCompleter;

    [Cmdlet(
      VerbsCommon.Get,
      "File",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
    )]
    [Alias("p")]
    [OutputType(typeof(string))]
    public class GetFile : WrappedCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipelineByPropertyName = true
      )]
      [SupportsWildcards]
      [RelativePathCompletions(
        "",
        PathItemType.File
      )]
      public string[]? Path;

      [Parameter(
        ParameterSetName = "LiteralPath",
        Mandatory = true,
        ValueFromPipelineByPropertyName = true
      )]
      [Alias("PSPath", "LP")]
      public string[]? LiteralPath;

      [Parameter(
        ParameterSetName = "Path",
        Position = 1
      )]
      [Parameter(
        ParameterSetName = "LiteralPath",
        Position = 1
      )]
      [SupportsWildcards]
      public string? Filter;

      [Parameter]
      [SupportsWildcards]
      public string[]? Include;

      [Parameter]
      [SupportsWildcards]
      public string[]? Exclude;

      [Parameter(
        ValueFromPipelineByPropertyName = true
      )]
      public long? ReadCount;

      [Parameter(
        ValueFromPipelineByPropertyName = true
      )]
      [Alias("First", "Head")]
      [ValidateRange(0, 9223372036854775807)]
      public long? TotalCount;

      [Parameter(
        ValueFromPipelineByPropertyName = true
      )]
      [Alias("Last")]
      [ValidateRange(0, 2147483647)]
      public int? Tail;

      [Parameter]
      public string? Delimiter;

      [Parameter]
      public string? Stream;

      [Parameter]
      public Encoding? Encoding;

      [Parameter]
      [Alias("f")]
      public SwitchParameter? Force;

      [Parameter]
      public SwitchParameter? AsByteStream;

      [Parameter]
      public SwitchParameter? Raw;

      [Parameter]
      public SwitchParameter? Wait;

      protected override string WrappedCommandName() => "Get-Content";
    }
  }
}
