namespace Module
{
  using System.IO;

  public abstract class LocalWrappedCommand : WrappedCommand
  {
    protected abstract string RelativeLocation();

    protected abstract string LocationRoot();

    protected string Reanchor(
      string typedPath = ""
    ) => Path.GetFullPath(
      typedPath,
      Path.GetFullPath(
        RelativeLocation(),
        LocationRoot()
      )
    );
  }
}
