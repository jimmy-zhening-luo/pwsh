using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Text.RegularExpressions;

namespace PathCompleter
{
  public enum PathItemType
  {
    Any,
    File,
    Directory
  }

  public static class PathSyntax {
    public static char EasyDirectorySeparator = '/';

    public static char NormalDirectorySeparator = '\\';

    public static string EasyDirectorySeparatorPattern = "/";

    public static string NormalDirectorySeparatorPattern = @"\\";

    public static string DuplicateDirectorySeparatorPattern = @"(?<!^)\\+";

    public static string DescendantPattern = @"^(?>[.\\]*)";

    public static string TildeRootedPattern = @"^~(?=\\|$)";

    public static string TildeRootPattern = @"^~(?>\\*)";
  }
}
