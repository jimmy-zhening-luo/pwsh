using System;
using System.IO;
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

    public string Root;
    public PathItemType Type;
    public bool Flat;
    public bool UseNativeDirectorySeparator;

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
      string typedPath,
      string separator
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
        NormalDirectorySeparator.ToString()
      );

      return FindPathCompletion(
        typedPath,
        UseNativeDirectorySeparator
          ? Path.DirectorySeparatorChar.ToString()
          : EasyDirectorySeparator
      );
    }
  }
}
