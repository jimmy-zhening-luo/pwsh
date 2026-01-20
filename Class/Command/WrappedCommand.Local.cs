namespace Module.Command
{
  using System.IO;

  public abstract class LocalWrappedCommand : WrappedCommand
  {
    protected abstract string LocationRoot
    {
      get;
    }

    protected virtual string RelativeLocation
    {
      get => "";
    }

    protected string Reanchor(
      string typedPath = ""
    ) => Path.GetFullPath(
      typedPath,
      Path.GetFullPath(
        RelativeLocation,
        LocationRoot
      )
    );
  }
}
