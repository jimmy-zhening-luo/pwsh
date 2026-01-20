global using static System.AttributeTargets;
global using static System.StringComparison;
global using static System.StringSplitOptions;
global using static System.Management.Automation.RunspaceMode;
global using static Completer.Escaper;

global using AttributeUsage = System.AttributeUsageAttribute;
global using IDictionary = System.Collections.IDictionary;
global using IStringEnumerable = System.Collections.Generic.IEnumerable<string>;
global using ICompletionEnumerable = System.Collections.Generic.IEnumerable<System.Management.Automation.CompletionResult>;
global using CommandAst = System.Management.Automation.Language.CommandAst;
