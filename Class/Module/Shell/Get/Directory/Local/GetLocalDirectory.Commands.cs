namespace Module.Shell.Get.Directory.Local
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
    public class GetDirectorySibling : GetLocalDirectoryCommand
    {
      [Parameter(
        ParameterSetName = "Items",
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
      public new string[]? Path;

      protected override string RelativeLocation => "..";

      protected override string LocationRoot => Pwd();
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
    public class GetDirectoryRelative : GetLocalDirectoryCommand
    {
      [Parameter(
        ParameterSetName = "Items",
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
      public new string[]? Path;

      protected override string RelativeLocation => @"..\..";

      protected override string LocationRoot => Pwd();
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
    public class GetDirectoryHome : GetLocalDirectoryCommand
    {
      [Parameter(
        ParameterSetName = "Items",
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
      public new string[]? Path;

      protected override string LocationRoot => Context.Home();
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
    public class GetDirectoryCode : GetLocalDirectoryCommand
    {
      [Parameter(
        ParameterSetName = "Items",
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
      public new string[]? Path;

      protected override string RelativeLocation => "code";

      protected override string LocationRoot => Context.Home();
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
    public class GetDirectoryDrive : GetLocalDirectoryCommand
    {
      [Parameter(
        ParameterSetName = "Items",
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
      public new string[]? Path;

      protected override string LocationRoot => Drive();
    }
  }
}
