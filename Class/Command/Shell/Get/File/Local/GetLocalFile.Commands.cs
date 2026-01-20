namespace Module.Command.Shell.Get.File.Local
{
  namespace Commands
  {

    [Cmdlet(
      VerbsCommon.Get,
      "FileSibling",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
    )]
    [Alias("p.")]
    [OutputType(typeof(string))]
    public class GetFileSibling : GetLocalFileCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipelineByPropertyName = true
      )]
      [SupportsWildcards]
      [RelativePathCompletions(
        "..",
        PathItemType.File
      )]
      public new string[]? Path;

      protected override string RelativeLocation => "..";

      protected override string LocationRoot => Pwd();
    }

    [Cmdlet(
      VerbsCommon.Get,
      "FileRelative",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
    )]
    [Alias("p..")]
    [OutputType(typeof(string))]
    public class GetFileRelative : GetLocalFileCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipelineByPropertyName = true
      )]
      [SupportsWildcards]
      [RelativePathCompletions(
        @"..\..",
        PathItemType.File
      )]
      public new string[]? Path;

      protected override string RelativeLocation => @"..\..";

      protected override string LocationRoot => Pwd();
    }

    [Cmdlet(
      VerbsCommon.Get,
      "FileHome",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
    )]
    [Alias("ph")]
    [OutputType(typeof(string))]
    public class GetFileHome : GetLocalFileCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipelineByPropertyName = true
      )]
      [SupportsWildcards]
      [PathCompletions(
        "~",
        PathItemType.File
      )]
      public new string[]? Path;

      protected override string LocationRoot => Home();
    }

    [Cmdlet(
      VerbsCommon.Get,
      "FileCode",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
    )]
    [Alias("pc")]
    [OutputType(typeof(string))]
    public class GetFileCode : GetLocalFileCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipelineByPropertyName = true
      )]
      [SupportsWildcards]
      [PathCompletions(
        @"~\code",
        PathItemType.File
      )]
      public new string[]? Path;

      protected override string RelativeLocation => "code";

      protected override string LocationRoot => Home();
    }

    [Cmdlet(
      VerbsCommon.Get,
      "FileDrive",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
    )]
    [Alias("p/")]
    [OutputType(typeof(string))]
    public class GetFileDrive : GetLocalFileCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipelineByPropertyName = true
      )]
      [SupportsWildcards]
      [RelativePathCompletions(
        @"\",
        PathItemType.File
      )]
      public new string[]? Path;

      protected override string LocationRoot => Drive();
    }
  }
}
