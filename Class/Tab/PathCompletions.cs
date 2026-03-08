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
  bool Flat = default,
  CompletionCase Case = CompletionCase.Lower
) : TCompletionsAttribute(Case)
{
  sealed public override Completers.PathCompleter Create() => new(
    Location,
    ItemType,
    Flat,
    Case
  );
}
