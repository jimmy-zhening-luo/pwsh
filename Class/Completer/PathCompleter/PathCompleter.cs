namespace Module.Completer.PathCompleter;

public sealed class PathCompleter : BaseCompleter
{
  public readonly string Root;

  public readonly PathItemType Type;

  public readonly bool Flat;

  public readonly bool Hidden;

  public readonly bool Reanchor;

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

  private static string Decanonicalize(
    string path,
    string location = "",
    string subpath = ""
  ) => Join(
    location,
    path,
    subpath
  )
    .Replace(
      '\\',
      '/'
    );

  public override IEnumerable<string> FulfillCompletion(
    string wordToComplete
  )
  {
    string pathToComplete = CanonicalizeAbsolutePath(
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
            Directory.Exists(
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
            && Directory.Exists(
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
    EnumerationOptions attributes = new()
    {
      IgnoreInaccessible = false
    };

    if (Hidden)
    {
      attributes.AttributesToSkip = FileAttributes.System;
    }

    if (Type == PathItemType.File)
    {
FileFirstMatch:
      foreach (
        string file in Directory.EnumerateFiles(
          location,
          filter,
          attributes
        )
      )
      {
        ++count;
        yield return Decanonicalize(
          GetFileName(
            file
          ),
          accumulatedSubpath
        );
      }

      if (
        count == 0
        && filter.Length > 1
        && attributes.AttributesToSkip != FileAttributes.System
      )
      {
        attributes.AttributesToSkip = FileAttributes.System;

        goto FileFirstMatch;
      }
    }

    int checkpoint = count;
    string directoryCap = Flat
      ? string.Empty
      : @"\";

Match:
    foreach (
      string directory in Directory.EnumerateDirectories(
        location,
        filter,
        attributes
      )
    )
    {
      ++count;
      yield return Decanonicalize(
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
      && attributes.AttributesToSkip != FileAttributes.System
    )
    {
      attributes.AttributesToSkip = FileAttributes.System;

      goto Match;
    }

    checkpoint = count;

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
        yield return Decanonicalize(
          GetFileName(
            file
          ),
          accumulatedSubpath
        );
      }

      if (
        count == checkpoint
        && filter.Length > 1
        && attributes.AttributesToSkip != FileAttributes.System
      )
      {
        attributes.AttributesToSkip = FileAttributes.System;

        goto Match;
      }
    }

    if (
      !string.IsNullOrEmpty(
        accumulatedSubpath
      )
    )
    {
      yield return Decanonicalize(
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
      yield return Decanonicalize(
        @"..\",
        accumulatedSubpath
      );
    }

    yield break;
  }
}
