using System;
using System.Management.Automation;
using Completer.PathCompleter;

namespace Shell
{
  namespace Commands
  {
    [Cmdlet(
      VerbsCommon.Clear,
      "Line",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true
    )]
    [Alias("cl", "clear")]
    [OutputType(typeof(void))]
    public class ClearLine : PSCmdlet
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0
      )]
      [AllowEmptyString]
      [SupportsWildcards]
      [AllowNull]
      [RelativePathCompletions]
      public string Path
      {
        get => path;
        set => path = value;
      }
      private string path = string.Empty;

      [Parameter(
        Position = 1
      )]
      [SupportsWildcards]
      public string Filter;

      [Parameter(
        ParameterSetName = "LiteralPath",
        Mandatory = true,
        ValueFromPipelineByPropertyName = true
      )]
      [Alias("PSPath", "LP")]
      public string[] LiteralPath;

      [Parameter]
      [SupportsWildcards]
      public string[] Include;

      [Parameter]
      [SupportsWildcards]
      public string[] Exclude;

      [Parameter]
      [Alias("f")]
      public SwitchParameter Force;

      [Parameter]
      public string Stream;

      private SteppablePipeline steppablePipeline = null;

      protected override void BeginProcessing()
      {
        if (
          path != string.Empty
          || ParameterSetName == "LiteralPath"
        )
        {
          using PowerShell ps = PowerShell.Create(
            RunspaceMode.CurrentRunspace
          );
          ps
            .AddCommand(
              SessionState
                .InvokeCommand
                .GetCommand(
                  "Clear-Content",
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

      protected override void ProcessRecord()
      {
        steppablePipeline?.Process();
      }

      protected override void EndProcessing()
      {
        if (steppablePipeline == null)
        {
          Console.Clear();
        }
        else
        {
          steppablePipeline.End();
          steppablePipeline.Dispose();
          steppablePipeline = null;
        }
      }
    }
  }
} // namespace Shell
