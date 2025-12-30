using System;

namespace Completer
{
  namespace PathCompleter
  {
    [AttributeUsage(
      AttributeTargets.Parameter
      | AttributeTargets.Property
      | AttributeTargets.Field
    )]
    public class PathCompletionsAttribute : BaseCompletionsAttribute<PathCompleter>
    {
      public readonly string Location;
      public readonly PathItemType ItemType;
      public readonly bool Flat;

      public PathCompletionsAttribute(string location) : base() => Location = location;

      public PathCompletionsAttribute(
        string location,
        PathItemType itemType
      ) : this(location) => ItemType = itemType;

      public PathCompletionsAttribute(
        string location,
        PathItemType itemType,
        bool flat
      ) : this(location, itemType) => Flat = flat;

      public override PathCompleter Create() => new(
        Location,
        ItemType,
        Flat,
        false
      );
    }
  } // namespace PathCompleter
} // namespace Completer
