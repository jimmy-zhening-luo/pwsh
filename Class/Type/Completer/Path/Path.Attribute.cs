using System;

namespace Completer
{
  namespace PathCompleter
  {
    [AttributeUsage(AttributeTargets.Parameter)]
    public class LocationPathCompletionsAttribute : BaseCompletionsAttribute
    {
      public readonly string Location;
      public readonly PathItemType ItemType;
      public readonly bool Flat;

      private LocationPathCompletionsAttribute() : base() { }

      public LocationPathCompletionsAttribute(string location) : this()
      {
        Location = location;
      }

      public LocationPathCompletionsAttribute(
        string location,
        PathItemType itemType
      ) : this(location)
      {
        ItemType = itemType;
      }

      public LocationPathCompletionsAttribute(
        string location,
        PathItemType itemType,
        bool flat
      ) : this(location, itemType)
      {
        Flat = flat;
      }

      public override PathCompleter Create()
      {
        return new PathCompleter(
          Location,
          ItemType,
          Flat
        );
      }
    }
  } // namespace PathCompleter
} // namespace Completer
