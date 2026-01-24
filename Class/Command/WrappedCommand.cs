namespace Module.Command;

public abstract class WrappedCommand(
  string WrappedCommandName
) : CoreCommand
{
  private SteppablePipeline? steppablePipeline = null;

  private protected virtual string Location => string.Empty;

  private protected virtual string LocationSubpath => string.Empty;

  private protected virtual bool NoSsh => false;

  private protected bool Here => string.IsNullOrEmpty(
    Location
  )
    && string.IsNullOrEmpty(
      LocationSubpath
    );

  protected sealed override void BeginProcessing()
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

      AddCommand(
        WrappedCommandName
      )
        .AddParameters(
          BoundParameters
        );

      Begin();
    }
  }

  protected sealed override void ProcessRecord()
  {
    if (!NoSsh || !Ssh)
    {
      steppablePipeline?.Process();
    }
  }

  private protected virtual void AnchorBoundPath()
  { }

  private protected virtual bool BeforeBeginProcessing() => true;

  private protected virtual void Default()
  { }

  private protected sealed override void AfterEndProcessing()
  {
    if (!NoSsh || !Ssh)
    {
      Default();
    }
  }

  private protected sealed override void Clean()
  {
    if (steppablePipeline != null)
    {
      steppablePipeline.End();
      steppablePipeline.Clean();
      steppablePipeline.Dispose();

      steppablePipeline = null;
    }
  }

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

  private void Begin()
  {
    if (powershell != null)
    {
      steppablePipeline = powershell.GetSteppablePipeline();
      steppablePipeline.Begin(
        this
      );
    }
  }
}
