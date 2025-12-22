using System;

namespace Completer
{
  namespace PathCompleter
  {
    [AttributeUsage(AttributeTargets.Parameter)]
    public class PathCompletionsAttribute : BaseCompletionsAttribute
    {
      public readonly string Location;
      public readonly PathItemType ItemType;
      public readonly bool Flat;

      private PathCompletionsAttribute() : base() { }

      public PathCompletionsAttribute(string location) : this()
      {
        Location = location;
      }

      public PathCompletionsAttribute(
        string location,
        PathItemType itemType
      ) : this(location)
      {
        ItemType = itemType;
      }

      public PathCompletionsAttribute(
        string location,
        PathItemType itemType,
        bool flat
      ) : this(location, itemType)
      {
        Flat = flat;
      }

      public override PathCompleter Create() => new(
        Location,
        ItemType,
        Flat
      );
    }
  } // namespace PathCompleter
} // namespace Completer
