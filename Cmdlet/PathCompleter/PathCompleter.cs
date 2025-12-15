using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Language;
using Completer;

namespace PathCompleter
{
  public enum PathItemType
  {
    Any,
    File,
    Directory
  }

  public class TestPathCompleter : CompleterBase
  {
    private readonly string Root;
    private readonly PathItemType Type;

    public TestPathCompleter(
      string root,
      PathItemType type,
      bool sort,
      bool surrounding
    )
    {
      Root = root;
      Type = type;
    }

    public override List<string> FulfillCompletion(
      string parameterName,
      string wordToComplete,
      IDictionary fakeBoundParameters
    )
    {
      return new List<string>();
    }
  }
}
