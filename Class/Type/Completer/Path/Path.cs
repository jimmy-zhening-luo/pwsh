using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Completer
{
  namespace PathCompleter
  {
    public class PathCompleter : BaseCompleter
    {
      public readonly string Root;
      public readonly PathItemType Type;
      public readonly bool Flat;

      private PathCompleter() : base() { }

      public PathCompleter(
        string root,
        PathItemType type,
        bool flat
      ) : this()
      {
        Root = Canonicalizer.AnchorHome(
          Canonicalizer.Normalize(
            Escaper.Unescape(root),
            Canonicalizer.PathSeparator,
            true,
            true
          )
        );
        Type = type;
        Flat = flat;
      }

      public IEnumerable<string> FindDescendant(string wordToComplete)
      {
        string currentPathValue = Canonicalizer.Normalize(
          wordToComplete,
          Canonicalizer.PathSeparator,
          true,
          false
        );
        string currentDirectoryValue = "";
        string searchLocation = "";
        string searchFilter = "";

        if (!string.IsNullOrWhiteSpace(currentPathValue))
        {
          int lastSeparatorIndex = currentPathValue.LastIndexOf(
            Canonicalizer.PathSeparatorChar
          );

          if (lastSeparatorIndex >= 0)
          {
            string beforeSeparator = currentPathValue[..lastSeparatorIndex];
            string afterSeparator = lastSeparatorIndex + 1 < currentPathValue.Length
              ? currentPathValue[(lastSeparatorIndex + 1)..]
              : string.Empty;

            if (!string.IsNullOrWhiteSpace(beforeSeparator))
            {
              currentDirectoryValue = beforeSeparator;
            }

            if (!string.IsNullOrWhiteSpace(afterSeparator))
            {
              searchFilter = afterSeparator;
            }
          }
          else
          {
            searchFilter = currentPathValue;
          }

          if (currentDirectoryValue != string.Empty)
          {
            string fullPathAndCurrentDirectory = Path.GetFullPath(
              currentDirectoryValue,
              Root
            );

            if (
              Directory.Exists(
                fullPathAndCurrentDirectory
              )
            )
            {
              searchLocation = fullPathAndCurrentDirectory;

            }
          }
        }

        if (searchLocation == string.Empty)
        {
          searchLocation = Root;
        }

        searchFilter = searchFilter + "*";
        EnumerationOptions attributes = new EnumerationOptions();

        attributes.IgnoreInaccessible = false;

        if (Type != PathItemType.File)
        {
          foreach (
            string directory in Directory.EnumerateDirectories(
              searchLocation,
              searchFilter,
              attributes
            )
          )
          {
            yield return Path.Join(
              currentDirectoryValue,
              Path.GetFileName(directory),
              Flat
                ? string.Empty
                : Canonicalizer.PathSeparator
            );
          }
        }

        if (Type != PathItemType.Directory)
        {
          foreach (
            string file in Directory.EnumerateFiles(
              searchLocation,
              searchFilter,
              attributes
            )
          )
          {
            yield return Path.Join(
              currentDirectoryValue,
              Path.GetFileName(file)
            );
          }
        }

        yield return Path.Join(
          currentDirectoryValue,
          @"..\"
        );

        if (currentDirectoryValue != string.Empty)
        {
          yield return Path.Join(
            currentDirectoryValue,
            Canonicalizer.PathSeparator
          );
        }

        yield break;
      }

      public override IEnumerable<string> FulfillArgumentCompletion(
        string parameterName,
        string wordToComplete,
        IDictionary fakeBoundParameters
      )
      {
        foreach (
          string descendant in FindDescendant(
            wordToComplete
          )
        )
        {
          yield return descendant.Replace(
            Canonicalizer.PathSeparatorChar,
            Canonicalizer.FriendlyPathSeparatorChar
          );
        }
      }
    }
  } // namespace PathCompleter
} // namespace Completer
