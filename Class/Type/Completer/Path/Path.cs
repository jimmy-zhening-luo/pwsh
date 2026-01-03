using System.IO;
using System.Collections.Generic;

namespace Completer.PathCompleter
{
  public class PathCompleter : BaseCompleter
  {
    public readonly string Root;
    public readonly PathItemType Type;
    public readonly bool Flat;
    public readonly bool Reanchor;

    public PathCompleter(
      string root,
      PathItemType type,
      bool flat,
      bool reanchor
    ) : base() => (Root, Type, Flat, Reanchor) =
    (
      Canonicalizer.AnchorHome(
        Canonicalizer.Normalize(root)
      ),
      type,
      flat,
      reanchor
    );

    public override IEnumerable<string> FulfillCompletion(string wordToComplete)
    {
      string pathToComplete = Canonicalizer
        .Normalize(wordToComplete, true)
        .Trim();
      string accumulatedSubpath = string.Empty;
      string location = string.Empty;
      string filter = string.Empty;

      while (pathToComplete != string.Empty)
      {
        int pathEnd = pathToComplete.LastIndexOf('\\');

        if (pathEnd < 0)
        {
          filter = pathToComplete;
          pathToComplete = string.Empty;
        }
        else
        {
          string subpathPart = pathToComplete[..pathEnd].Trim();

          int wordStart = pathEnd + 1;

          if (wordStart < pathToComplete.Length)
          {
            filter = pathToComplete[wordStart..].Trim();
          }

          if (subpathPart == string.Empty)
          {
            pathToComplete = string.Empty;
          }
          else
          {
            string anchoredPath = Path.GetFullPath(
              subpathPart,
              Root
            );

            if (Directory.Exists(anchoredPath))
            {
              accumulatedSubpath = Path.GetRelativePath(
                Root,
                anchoredPath
              );
              location = anchoredPath;
              pathToComplete = string.Empty;
            }
            else if (
              Reanchor
              && Directory.Exists(subpathPart)
            )
            {
              accumulatedSubpath = Path.GetFullPath(subpathPart);
              location = accumulatedSubpath;
              pathToComplete = string.Empty;
            }
            else
            {
              filter = string.Empty;
              pathToComplete = subpathPart;
            }
          }
        }
      }

      if (location == string.Empty)
      {
        location = Root;
      }

      int count = 0;
      filter += "*";
      EnumerationOptions attributes = new()
      {
        IgnoreInaccessible = false
      };

      if (Type == PathItemType.File)
      {
        foreach (
          string file in Directory.EnumerateFiles(
            location,
            filter,
            attributes
          )
        )
        {
          ++count;
          yield return Canonicalizer.Denormalize(
            Path.GetFileName(file),
            accumulatedSubpath
          );
        }
      }

      string directoryCap = Flat
        ? string.Empty
        : @"\";

      foreach (
        string directory in Directory.EnumerateDirectories(
          location,
          filter,
          attributes
        )
      )
      {
        ++count;
        yield return Canonicalizer.Denormalize(
          Path.GetFileName(directory),
          accumulatedSubpath,
          directoryCap
        );
      }

      if (Type == PathItemType.Any)
      {
        foreach (
          string file in Directory.EnumerateFiles(
            location,
            filter,
            attributes
          )
        )
        {
          ++count;
          yield return Canonicalizer.Denormalize(
            Path.GetFileName(file),
            accumulatedSubpath
          );
        }
      }

      if (accumulatedSubpath != string.Empty)
      {
        yield return Canonicalizer.Denormalize(
          @"\",
          accumulatedSubpath
        );
      }

      if (
        accumulatedSubpath != string.Empty
        || count != 0
      )
      {
        yield return Canonicalizer.Denormalize(
          @"..\",
          accumulatedSubpath
        );
      }

      yield break;
    }
  }
}
