using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Text.RegularExpressions;
using Completer;

namespace PathCompleter
{
  public enum PathItemType
  {
    Any,
    File,
    Directory
  }

  public abstract class PathCompleterCore : CompleterBase
  {
    public static char EasyDirectorySeparator = '/';

    public static char NormalDirectorySeparator = '\\';

    public static string DuplicateDirectorySeparatorPattern = @"(?<!^)\\+";

    private readonly string Root;
    private readonly PathItemType Type;
    private readonly bool Flat;
    private readonly bool UseNativeDirectorySeparator;

    public PathCompleterCore(
      string root,
      PathItemType type,
      bool flat,
      bool useNativeDirectorySeparator
    )
    {
      Root = root;
      Type = type;
      Flat = flat;
      UseNativeDirectorySeparator = useNativeDirectorySeparator;
    }

    public abstract List<string> FindPathCompletion(
      string typedPath
    );

    public override List<string> FulfillCompletion(
      string parameterName,
      string wordToComplete,
      IDictionary fakeBoundParameters
    )
    {
      string unescapedWordToComplete = Unescape(wordToComplete);

      string normalizedWordToComplete = unescapedWordToComplete.Replace(
        EasyDirectorySeparator,
        NormalDirectorySeparator
      );

      string trimmedWordToComplete = Regex.Replace(
        normalizedWordToComplete,
        DuplicateDirectorySeparatorPattern,
        NormalDirectorySeparator.ToString()
      );

      return FindPathCompletion(
        trimmedWordToComplete
      );
    }
  }
}
