global using System.Management.Automation;
global using Module.Input.PathResolution;
global using Module.Completer;
global using Module.Completer.PathCompleter;

global using static System.Math;
global using static System.Management.Automation.RunspaceMode;
global using static Module.Context;
global using static Module.Input.Escaper;
global using static Module.Input.PathResolution.Canonicalizer;

global using Uri = System.Uri;
global using Enum = System.Enum;
global using StringComparer = System.StringComparer;
global using StringComparison = System.StringComparison;
global using AttributeUsage = System.AttributeUsageAttribute;
global using AttributeTargets = System.AttributeTargets;
global using IDictionary = System.Collections.IDictionary;
global using IStringEnumerable = System.Collections.Generic.IEnumerable<string>;
global using ICompletionEnumerable = System.Collections.Generic.IEnumerable<System.Management.Automation.CompletionResult>;
global using StringHashSet = System.Collections.Generic.HashSet<string>;
global using SortedStringSet = System.Collections.Generic.SortedSet<string>;
global using CommandAst = System.Management.Automation.Language.CommandAst;
