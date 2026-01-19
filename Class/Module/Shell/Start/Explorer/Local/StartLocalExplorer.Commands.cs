namespace Module.Shell.Start.Explorer.Local
{
  namespace Commands
  {
    using System.Management.Automation;
    using Completer.PathCompleter;

    [Cmdlet(
      VerbsLifecycle.Start,
      "ExplorerSibling",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
    )]
    [Alias("e.")]
    [OutputType(typeof(void))]
    public class StartExplorerSibling : StartLocalExplorerCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyCollection]
      [SupportsWildcards]
      [RelativePathCompletions(
        ".."
      )]
      public new string[]? Path;

      protected override string RelativeLocation => "..";

      protected override string LocationRoot => Pwd();
    }

    [Cmdlet(
      VerbsLifecycle.Start,
      "ExplorerRelative",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
    )]
    [Alias("e..")]
    [OutputType(typeof(void))]
    public class StartExplorerRelative : StartLocalExplorerCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyCollection]
      [SupportsWildcards]
      [RelativePathCompletions(
        @"..\.."
      )]
      public new string[]? Path;

      protected override string RelativeLocation => @"..\..";

      protected override string LocationRoot => Pwd();
    }

    [Cmdlet(
      VerbsLifecycle.Start,
      "ExplorerHome",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
    )]
    [Alias("eh")]
    [OutputType(typeof(void))]
    public class StartExplorerHome : StartLocalExplorerCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyCollection]
      [SupportsWildcards]
      [PathCompletions(
        "~"
      )]
      public new string[]? Path;

      protected override string LocationRoot => Context.Home();
    }

    [Cmdlet(
      VerbsLifecycle.Start,
      "ExplorerCode",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
    )]
    [Alias("ec")]
    [OutputType(typeof(void))]
    public class StartExplorerCode : StartLocalExplorerCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyCollection]
      [SupportsWildcards]
      [PathCompletions(
        @"~\code"
      )]
      public new string[]? Path;

      protected override string RelativeLocation => "code";

      protected override string LocationRoot => Context.Home();
    }

    [Cmdlet(
      VerbsLifecycle.Start,
      "ExplorerDrive",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
    )]
    [Alias("e/")]
    [OutputType(typeof(void))]
    public class StartExplorerDrive : StartLocalExplorerCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyCollection]
      [SupportsWildcards]
      [RelativePathCompletions(
        @"\"
      )]
      public new string[]? Path;

      protected override string LocationRoot => Drive();
    }
  }
}
