namespace Module.Tab;

internal enum PathItemType
{
  Any,
  File,
  Directory,
}

internal class PathCompletionsAttribute(
  string Location = "",
  PathItemType ItemType = default,
  bool Flat = default
) : TCompletionsAttribute
{
  public sealed override Completers.PathCompleter Create() => new(
    Location,
    ItemType,
    Flat
  );
}
