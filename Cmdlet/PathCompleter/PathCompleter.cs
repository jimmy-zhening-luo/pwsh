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
    public static string EasyDirectorySeparator = "/";

    public static string NormalDirectorySeparator = @"\";

    public static string DuplicateDirectorySeparatorPattern = @"(?<!^)\\+";

    public abstract List<string> FindPathCompletion(
      string typedPath
    );

    public override List<string> FulfillCompletion(
      string parameterName,
      string wordToComplete,
      IDictionary fakeBoundParameters
    )
    {
      string normalizedWordToComplete = Unescape(wordToComplete).Replace(
        EasyDirectorySeparator,
        NormalDirectorySeparator
      );
      string typedPath = Regex.Replace(
        normalizedWordToComplete,
        DuplicateDirectorySeparatorPattern,
        NormalDirectorySeparator
      );

      return FindPathCompletion(
        typedPath
      );
    }
  }
}
