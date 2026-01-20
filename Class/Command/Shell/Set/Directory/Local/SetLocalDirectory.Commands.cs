namespace Module.Shell.Set.Directory.Local
{
  namespace Commands
  {
    using System.Management.Automation;

    [Cmdlet(
      VerbsCommon.Set,
      "DirectorySibling",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("c.")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDirectorySibling : SetLocalDirectoryCommand
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
        "..",
        PathItemType.Directory
      )]
      public new string? Path;

      protected override string RelativeLocation => "..";

      protected override string LocationRoot => Pwd();
    }

    [Cmdlet(
      VerbsCommon.Set,
      "DirectoryRelative",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("c..")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDirectoryRelative : SetLocalDirectoryCommand
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
        @"..\..",
        PathItemType.Directory
      )]
      public new string? Path;

      protected override string RelativeLocation => @"..\..";

      protected override string LocationRoot => Pwd();
    }

    [Cmdlet(
      VerbsCommon.Set,
      "DirectoryHome",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("ch")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDirectoryHome : SetLocalDirectoryCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyString]
      [SupportsWildcards]
      [PathCompletions(
        "~",
        PathItemType.Directory
      )]
      public new string? Path;

      protected override string LocationRoot => Home();
    }

    [Cmdlet(
      VerbsCommon.Set,
      "DirectoryCode",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("cc")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDirectoryCode : SetLocalDirectoryCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyString]
      [SupportsWildcards]
      [PathCompletions(
        @"~\code",
        PathItemType.Directory
      )]
      public new string? Path;

      protected override string RelativeLocation => "code";

      protected override string LocationRoot => Home();
    }

    [Cmdlet(
      VerbsCommon.Set,
      "Drive",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("c/")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDrive : SetLocalDirectoryCommand
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
        @"\",
        PathItemType.Directory
      )]
      public new string? Path;

      protected override string LocationRoot => Drive();
    }

    [Cmdlet(
      VerbsCommon.Set,
      "DriveD",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("d/")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDriveD : SetLocalDirectoryCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyString]
      [SupportsWildcards]
      [PathCompletions(
        "D:",
        PathItemType.Directory
      )]
      public new string? Path;

      protected override string LocationRoot => @"D:\";
    }
  }
}
