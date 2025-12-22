using System;

namespace Completer
{
  namespace PathCompleter
  {
    [AttributeUsage(AttributeTargets.Parameter)]
    public class LocationPathCompletionsAttribute(
      string Location,
      PathItemType ItemType,
      bool Flat
    ) : BaseCompletionsAttribute
    {
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
