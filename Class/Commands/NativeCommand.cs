namespace Module.Commands;

public abstract partial class NativeCommand(
  bool CreateProcess = default,
  bool SkipSsh = default
) : CoreCommand(SkipSsh)
{
  private protected static bool IsNativeArgument(string argument) => NativeArgumentRegex().IsMatch(argument);
  [System.Text.RegularExpressions.GeneratedRegex(
    @"^(?>-(?>[A-Za-z]|(?>(?>-[A-Za-z][A-Za-z\d]*(?>_[A-Za-z\d]+)*)(?>-[A-Za-z\d]+(?>_[A-Za-z\d]+)*)*)))(?>=\S+)?$"
  )]
  private static partial System.Text.RegularExpressions.Regex NativeArgumentRegex();

  private protected record SwitchBoard(
    bool D = default,
    bool E = default,
    bool I = default,
    bool O = default,
    bool P = default,
    bool V = default
  );
  private protected virtual SwitchBoard Uppercase => new();

  private protected readonly List<string> NativeArguments = [];

  [Parameter(
    Position = 100,
    ValueFromRemainingArguments = true,
    DontShow = true,
    HelpMessage = "Additional arguments"
  )]
  [Tab.Path.PathCompletions]
  public string[] ArgumentList { get; set; } = [];

  [Parameter(
    HelpMessage = "When execution results in a non-zero exit code, warn and continue instead of the default behavior of throwing a terminating error"
  )]
  public SwitchParameter NoThrow { get; set; }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -v flag as argument"
  )]
  public SwitchParameter V { get; set; }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -d flag as argument"
  )]
  public SwitchParameter D { get; set; }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -E flag as argument"
  )]
  public SwitchParameter E { get; set; }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -i flag as argument"
  )]
  public SwitchParameter I { get; set; }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -o flag as argument"
  )]
  public SwitchParameter O { get; set; }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -P flag as argument"
  )]
  public SwitchParameter P { get; set; }

  private protected abstract string CommandPath { get; }

  private protected abstract List<string> BuildNativeCommand();

  private protected virtual void PreprocessArguments()
  { }

  private protected sealed override void Preprocess()
  {
    List<string> arguments = [];

    foreach (var argument in ArgumentList)
    {
      if (IsNativeArgument(argument))
      {
        NativeArguments.Add(argument);
      }
      else
      {
        arguments.Add(argument);
      }
    }

    ArgumentList = [.. arguments];

    PreprocessArguments();
  }

  private protected sealed override void Postprocess()
  {
    List<string> arguments = BuildNativeCommand();

    if (D)
    {
      arguments.Add(Uppercase.D ? "-D" : "-d");
    }
    if (E)
    {
      arguments.Add(Uppercase.E ? "-E" : "-e");
    }
    if (I)
    {
      arguments.Add(Uppercase.I ? "-I" : "-i");
    }
    if (O)
    {
      arguments.Add(Uppercase.O ? "-O" : "-o");
    }
    if (P)
    {
      arguments.Add(Uppercase.P ? "-P" : "-p");
    }
    if (V)
    {
      arguments.Add(Uppercase.V ? "-V" : "-v");
    }

    arguments.AddRange(ArgumentList);
    arguments.AddRange(NativeArguments);

    List<string> escapedArguments = [];

    foreach (var word in arguments)
    {
      escapedArguments.Add(
        Client.Console.String.EscapeDoubleQuoted(word)
      );
    }

    if (CreateProcess)
    {
      Client.Start.CreateProcess(
        CommandPath,
        escapedArguments,
        noNewWindow: true
      );
    }
    else
    {
      List<string> command = [
        "&",
        Client.Console.String.EscapeDoubleQuoted(CommandPath),
        .. escapedArguments,
      ];

      AddScript(string.Join(Client.Console.String.Space, command));

      ProcessSteppablePipeline();
      EndSteppablePipeline();

      if (HadNativeError)
      {
        if (noThrow)
        {
          WriteWarning("Execution error");
        }
        else
        {
          ThrowError("Execution error");
        }
      }
    }
  }
}
