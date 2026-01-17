namespace Module.Shell.Set
{
  namespace Commands
  {
    using System.Management.Automation;
    using Completer.PathCompleter;

    [Cmdlet(
      VerbsCommon.Set,
      "DirectorySibling",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("c.")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDirectorySibling : SetDirectoryLocation
    {
      [Parameter(
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
      public new string Path;

      protected override string location() => "..";

      protected override string root() => Pwd();
    }

    [Cmdlet(
      VerbsCommon.Set,
      "DirectoryRelative",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("c..")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDirectoryRelative : SetDirectoryLocation
    {
      [Parameter(
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
      public new string Path;

      protected override string location() => @"..\..";

      protected override string root() => Pwd();
    }

    [Cmdlet(
      VerbsCommon.Set,
      "DirectoryHome",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("ch")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDirectoryHome : SetDirectoryLocation
    {
      [Parameter(
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
      public new string Path;

      protected override string location() => "";

      protected override string root() => Context.Home();
    }

    [Cmdlet(
      VerbsCommon.Set,
      "DirectoryCode",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("cc")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDirectoryCode : SetDirectoryLocation
    {
      [Parameter(
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
      public new string Path;

      protected override string location() => "code";

      protected override string root() => Context.Home();
    }

    [Cmdlet(
      VerbsCommon.Set,
      "Drive",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("c/")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDrive : SetDirectoryLocation
    {
      [Parameter(
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
      public new string Path;

      protected override string location() => "";

      protected override string root() => Drive();
    }

    [Cmdlet(
      VerbsCommon.Set,
      "DriveD",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("d/")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDriveD : SetDirectoryLocation
    {
      [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyString]
      [SupportsWildcards]
      [RelativePathCompletions(
        "D:",
        PathItemType.Directory
      )]
      public new string Path;

      protected override string location() => "";

      protected override string root() => @"D:\";
    }
  }
}
