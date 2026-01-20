namespace Module.Command;

using static System.IO.Path;

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
  ) => GetFullPath(
    typedPath,
    GetFullPath(
      RelativeLocation,
      LocationRoot
    )
  );
}
