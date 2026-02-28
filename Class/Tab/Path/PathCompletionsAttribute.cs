namespace Module.Tab.Path;

public class PathCompletionsAttribute(
  string Location = "",
  PathItemType ItemType = default
) : TabCompletionsAttribute<PathCompleter>
{
  public bool Flat { get; init; }

  public bool IncludeHidden { get; init; }

  public sealed override PathCompleter Create() => new(
    Location,
    ItemType,
    Flat,
    IncludeHidden
  );
}
