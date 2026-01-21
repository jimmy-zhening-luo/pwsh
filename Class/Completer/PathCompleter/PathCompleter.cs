namespace Module.Completer.PathCompleter;

public class PathCompleter : BaseCompleter
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

  public override IEnumerable<string> FulfillCompletion(
    string wordToComplete
  )
  {
    string pathToComplete = Normalize(
      wordToComplete,
      true
    )
      .Trim();
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
        string subpathPart = pathToComplete[..pathEnd].Trim();

        int wordStart = pathEnd + 1;

        if (wordStart < pathToComplete.Length)
        {
          filter = pathToComplete[wordStart..].Trim();
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
          string anchoredPath = Path.GetFullPath(
            subpathPart,
            Root
          );

          if (
            Directory.Exists(
              anchoredPath
            )
          )
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
            && Directory.Exists(
              subpathPart
            )
          )
          {
            accumulatedSubpath = Path.GetFullPath(
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
        yield return Denormalize(
          Path.GetFileName(
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
      yield return Denormalize(
        Path.GetFileName(
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
        yield return Denormalize(
          Path.GetFileName(
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
      yield return Denormalize(
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
      yield return Denormalize(
        @"..\",
        accumulatedSubpath
      );
    }

    yield break;
  }
}
