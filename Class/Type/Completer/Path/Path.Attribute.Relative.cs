using System;
using System.Management.Automation;

namespace Completer
{
  namespace PathCompleter
  {
    [AttributeUsage(AttributeTargets.Parameter)]
    public class RelativePathCompletionsAttribute(
      ScriptBlock CurrentDirectory,
      PathItemType? ItemType,
      bool? Flat
    ) : ArgumentCompleterAttribute, IArgumentCompleterFactory
    {
      public IArgumentCompleter Create()
      {
        return new PathCompleter(
          CurrentDirectory
            .Invoke()[0]
            .BaseObject
            .ToString(),
          ItemType,
          Flat
        );
      }
    }
  } // namespace PathCompleter
} // namespace Completer
