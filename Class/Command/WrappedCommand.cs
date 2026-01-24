namespace Module.Command;

public abstract class WrappedCommand(
  string WrappedCommandName
) : CoreCommand
{
  private protected virtual string Location => string.Empty;

  private protected virtual string LocationSubpath => string.Empty;

  private protected virtual bool NoSsh => false;

  private protected bool Here => string.IsNullOrEmpty(
    Location
  )
    && string.IsNullOrEmpty(
      LocationSubpath
    );

  private SteppablePipeline? steppablePipeline = null;

  protected override void BeginProcessing()
  {
    if (
      (
        !NoSsh
        || !Ssh
      )
      && BeforeBeginProcessing()
    )
    {
      AnchorBoundPath();
      Begin(
        AddCommand(
          WrappedCommandName
        )
          .AddParameters(
            BoundParameters
          )
      );
    }
  }

  protected override void ProcessRecord()
  {
    if (!NoSsh || !Ssh)
    {
      steppablePipeline?.Process();
    }
  }

  protected override void EndProcessing()
  {
    if (!NoSsh || !Ssh)
    {
      BeforeEndProcessing();
    }

    Clean();
  }

  private protected virtual void AnchorBoundPath()
  { }

  private protected virtual bool BeforeBeginProcessing() => true;

  private protected virtual void BeforeEndProcessing()
  { }

  private protected string Reanchor(
    string typedPath = ""
  ) => Path.GetFullPath(
    typedPath,
    Path.GetFullPath(
      LocationSubpath,
      string.IsNullOrEmpty(
        Location
      )
        ? Pwd()
        : Location
    )
  );

  private void Begin(
    PowerShell ps
  )
  {
    steppablePipeline = ps.GetSteppablePipeline();
    steppablePipeline.Begin(
      this
    );
  }

  private void Clean()
  {
    if (steppablePipeline != null)
    {
      steppablePipeline.End();
      steppablePipeline.Clean();
      steppablePipeline.Dispose();
      steppablePipeline = null;
    }
  }
}
