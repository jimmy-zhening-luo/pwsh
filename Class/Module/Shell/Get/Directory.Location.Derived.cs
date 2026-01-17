namespace Module.Shell.Get
{
  namespace Commands
  {
    using System.IO;
    using System.Management.Automation;
    using Completer.PathCompleter;

    [Cmdlet(
      VerbsCommon.Get,
      "DirectorySibling",
      DefaultParameterSetName = "Items",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
    )]
    [Alias("l.")]
    [OutputType(
      typeof(DirectoryInfo),
      typeof(FileInfo)
    )]
    public class GetDirectorySibling : GetDirectoryLocation
    {
      [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyCollection]
      [SupportsWildcards]
      [RelativePathCompletions(
        "..",
        PathItemType.Directory
      )]
      public new string[] Path;

      protected override string location() => "..";

      protected override string root() => Pwd();
    }

    [Cmdlet(
      VerbsCommon.Get,
      "DirectoryRelative",
      DefaultParameterSetName = "Items",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
    )]
    [Alias("l..")]
    [OutputType(
      typeof(DirectoryInfo),
      typeof(FileInfo)
    )]
    public class GetDirectoryRelative : GetDirectoryLocation
    {
      [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyCollection]
      [SupportsWildcards]
      [RelativePathCompletions(
        @"..\..",
        PathItemType.Directory
      )]
      public new string[] Path;

      protected override string location() => @"..\..";

      protected override string root() => Pwd();
    }

    [Cmdlet(
      VerbsCommon.Get,
      "DirectoryHome",
      DefaultParameterSetName = "Items",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
    )]
    [Alias("lh")]
    [OutputType(
      typeof(DirectoryInfo),
      typeof(FileInfo)
    )]
    public class GetDirectoryHome : GetDirectoryLocation
    {
      [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyCollection]
      [SupportsWildcards]
      [PathCompletions(
        "~",
        PathItemType.Directory
      )]
      public new string[] Path;

      protected override string location() => "";

      protected override string root() => Context.Home();
    }

    [Cmdlet(
      VerbsCommon.Get,
      "DirectoryCode",
      DefaultParameterSetName = "Items",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
    )]
    [Alias("lc")]
    [OutputType(
      typeof(DirectoryInfo),
      typeof(FileInfo)
    )]
    public class GetDirectoryCode : GetDirectoryLocation
    {
      [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyCollection]
      [SupportsWildcards]
      [PathCompletions(
        @"~\code",
        PathItemType.Directory
      )]
      public new string[] Path;

      protected override string location() => "code";

      protected override string root() => Context.Home();
    }

    [Cmdlet(
      VerbsCommon.Get,
      "DirectoryDrive",
      DefaultParameterSetName = "Items",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
    )]
    [Alias("l/")]
    [OutputType(
      typeof(DirectoryInfo),
      typeof(FileInfo)
    )]
    public class GetDirectoryDrive : GetDirectoryLocation
    {
      [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyCollection]
      [SupportsWildcards]
      [RelativePathCompletions(
        @"\",
        PathItemType.Directory
      )]
      public new string[] Path;

      protected override string location() => "";

      protected override string root() => Drive();
    }
  }
}
