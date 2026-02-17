namespace Module.Completer.Path;

using static System.IO.Path;

public sealed class PathCompleter : BaseCompleter
{
  private readonly string Root;

  private readonly PathItemType Type;

  private readonly bool Flat;

  private readonly bool Hidden;

  private readonly bool Reanchor;

  public PathCompleter(
    string root,
    PathItemType type,
    bool flat,
    bool hidden,
    bool reanchor
  ) : base() => (
    Root,
    Type,
    Flat,
    Hidden,
    Reanchor
  ) = (
    root,
    type,
    flat,
    hidden,
    reanchor
  );

  public override IEnumerable<string> FulfillCompletion(
    string wordToComplete
  )
  {
    string pathToComplete = Client.FileSystem.PathString.Normalize(
      wordToComplete,
      true
    );
    string accumulatedSubpath = string.Empty;
    string location = string.Empty;
    string filter = string.Empty;

    while (
      !string.IsNullOrEmpty(
        pathToComplete
      )
    )
    {
      int pathEnd = pathToComplete.LastIndexOf(
        '\\'
      );

      if (pathEnd < 0)
      {
        filter = pathToComplete;
        pathToComplete = string.Empty;
      }
      else
      {
        string subpathPart = pathToComplete[..pathEnd]
          .Trim();

        int wordStart = pathEnd + 1;

        if (wordStart < pathToComplete.Length)
        {
          filter = pathToComplete[wordStart..]
            .Trim();
        }

        if (
          string.IsNullOrEmpty(
            subpathPart
          )
        )
        {
          pathToComplete = string.Empty;
        }
        else
        {
          string anchoredPath = GetFullPath(
            subpathPart,
            Root
          );

          if (
            System.IO.Directory.Exists(
              anchoredPath
            )
          )
          {
            accumulatedSubpath = GetRelativePath(
              Root,
              anchoredPath
            );
            location = anchoredPath;
            pathToComplete = string.Empty;
          }
          else if (
            Reanchor
            && System.IO.Directory.Exists(
              subpathPart
            )
          )
          {
            accumulatedSubpath = GetFullPath(
              subpathPart
            );
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

    if (
      string.IsNullOrEmpty(
        location
      )
    )
    {
      location = Root;
    }

    int count = 0;
    filter += "*";
    var attributes = new System.IO.EnumerationOptions()
    {
      IgnoreInaccessible = false
    };

    if (Hidden)
    {
      attributes.AttributesToSkip = System.IO.FileAttributes.System;
    }

    if (Type == PathItemType.File)
    {
FileFirstMatch:
      foreach (
        var file in System.IO.Directory.EnumerateFiles(
          location,
          filter,
          attributes
        )
      )
      {
        ++count;
        yield return Canonicalizer.Denormalize(
          GetFileName(
            file
          ),
          accumulatedSubpath
        );
      }

      if (
        count == 0
        && filter.Length > 1
        && attributes.AttributesToSkip != System.IO.FileAttributes.System
      )
      {
        attributes.AttributesToSkip = System.IO.FileAttributes.System;

        goto FileFirstMatch;
      }
    }

    int checkpoint = count;
    string directoryCap = Flat
      ? string.Empty
      : @"\";

Match:
    foreach (
      var directory in System.IO.Directory.EnumerateDirectories(
        location,
        filter,
        attributes
      )
    )
    {
      ++count;
      yield return Canonicalizer.Denormalize(
        GetFileName(
          directory
        ),
        accumulatedSubpath,
        directoryCap
      );
    }

    if (
      count == checkpoint
      && filter.Length > 1
      && attributes.AttributesToSkip != System.IO.FileAttributes.System
    )
    {
      attributes.AttributesToSkip = System.IO.FileAttributes.System;

      goto Match;
    }

    checkpoint = count;

    if (Type == PathItemType.Any)
    {
      foreach (
        var file in System.IO.Directory.EnumerateFiles(
          location,
          filter,
          attributes
        )
      )
      {
        ++count;
        yield return Canonicalizer.Denormalize(
          GetFileName(
            file
          ),
          accumulatedSubpath
        );
      }

      if (
        count == checkpoint
        && filter.Length > 1
        && attributes.AttributesToSkip != System.IO.FileAttributes.System
      )
      {
        attributes.AttributesToSkip = System.IO.FileAttributes.System;

        goto Match;
      }
    }

    if (
      !string.IsNullOrEmpty(
        accumulatedSubpath
      )
    )
    {
      yield return Canonicalizer.Denormalize(
        @"\",
        accumulatedSubpath
      );
    }

    if (
      !string.IsNullOrEmpty(
        accumulatedSubpath
      )
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
