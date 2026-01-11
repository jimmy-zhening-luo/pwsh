namespace Module.Shell.Set.Commands
{
  using System;
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
    typeof(System.Management.Automation.PathInfo),
    typeof(System.Management.Automation.PathInfoStack)
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

    protected override string Reanchor(string typedPath) => System.IO.Path.GetFullPath(
      typedPath,
      System.IO.Path.GetFullPath(
        "..",
        Pwd()
      )
    );
  }

  [Cmdlet(
    VerbsCommon.Set,
    "DirectoryRelative",
    SupportsTransactions = true,
    HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
  )]
  [Alias("c..")]
  [OutputType(
    typeof(System.Management.Automation.PathInfo),
    typeof(System.Management.Automation.PathInfoStack)
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
      @"..\..",
      PathItemType.Directory
    )]
    public new string Path;

    protected override string Reanchor(string typedPath) => System.IO.Path.GetFullPath(
      typedPath,
      System.IO.Path.GetFullPath(
        @"..\..",
        Pwd()
      )
    );
  }
}
