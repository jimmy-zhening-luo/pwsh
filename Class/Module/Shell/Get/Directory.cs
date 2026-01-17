namespace Module.Shell.Get
{
  namespace Commands
  {
    using System.IO;
    using System.Management.Automation;
    using Completer.PathCompleter;

    [Cmdlet(
      VerbsCommon.Get,
      "Directory",
      DefaultParameterSetName = "Items",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
    )]
    [Alias("l")]
    [OutputType(
      typeof(DirectoryInfo),
      typeof(FileInfo)
    )]
    public class GetDirectory : WrappedCommand
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
        "",
        PathItemType.Directory
      )]
      public string[] Path;

      [Parameter(
        ParameterSetName = "LiteralItems",
        Mandatory = true,
        ValueFromPipelineByPropertyName = true
      )]
      [Alias("PSPath", "LP")]
      public string[] LiteralPath;

      [Parameter(
        ParameterSetName = "Items",
        Position = 1
      )]
      [Parameter(
        ParameterSetName = "LiteralItems",
        Position = 1
      )]
      [SupportsWildcards]
      public string Filter;

      [Parameter]
      [SupportsWildcards]
      public string[] Include;

      [Parameter]
      [SupportsWildcards]
      public string[] Exclude;

      [Parameter]
      [Alias("s", "r")]
      public SwitchParameter Recurse;

      [Parameter]
      public uint Depth;

      [Parameter]
      [Alias("f")]
      public SwitchParameter Force;

      [Parameter]
      public SwitchParameter Name;

      [Parameter]
      [Alias("ad")]
      public SwitchParameter Directory;

      [Parameter]
      [Alias("af")]
      public SwitchParameter File;

      [Parameter]
      [Alias("ah", "h")]
      public SwitchParameter Hidden;

      [Parameter]
      [Alias("as")]
      public SwitchParameter System;

      [Parameter]
      [Alias("ar")]
      public SwitchParameter ReadOnly;

      [Parameter]
      public SwitchParameter FollowSymlink;

      [Parameter]
      public FlagsExpression<FileAttributes> Attributes;

      protected override void BeginProcessing()
      {
        using PowerShell ps = PowerShell.Create(
          RunspaceMode.CurrentRunspace
        );
        ps
          .AddCommand(
            SessionState
              .InvokeCommand
              .GetCommand(
                "Get-ChildItem",
                CommandTypes.Cmdlet
              )
          )
          .AddParameters(
            MyInvocation.BoundParameters
          );

        steppablePipeline = ps.GetSteppablePipeline();
        steppablePipeline.Begin(this);
      }
    }
  }
}
