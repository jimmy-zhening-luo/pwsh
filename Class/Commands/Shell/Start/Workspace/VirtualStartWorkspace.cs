namespace PowerModule.Commands.Shell.Start.Workspace;

abstract public class VirtualStartWorkspace() : CoreCommand(true)
{
  Client.File.Handler.EditorProfile profile;

  abstract public string Path
  { private protected get; init; }

  [Parameter(Position = 1)]
  [ValidateNotNullOrWhiteSpace]
  [Tab.EnumCompletions(typeof(Client.File.Handler.EditorProfile))]
  public string Name
  { private get; init; } = string.Empty;

  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819: Properties should not return arrays", Justification = "PowerShell: Required to bind parameter values from remaining arguments as a list of values.")]
  [Parameter(
    Position = 2,
    ValueFromRemainingArguments = true,
    DontShow = true,
    HelpMessage = "Additional arguments"
  )]
  [ValidateLength(1, int.MaxValue)]
  [Tab.PathCompletions]
  public string[] ArgumentList
  {
    init => arguments = new(value);
  }
  LinkedList<string> arguments = [];

  [Parameter]
  public SwitchParameter Window
  {
    private get => window is Client.File.Handler.EditorWindow.New;
    set => window = Client.File.Handler.EditorWindow.New;
  }
  Client.File.Handler.EditorWindow window;

  [Parameter]
  public SwitchParameter ReuseWindow
  { private get; init; }

  sealed override private protected void Preprocess()
  {
    if (ReuseWindow && !Window)
    {
      window = Client.File.Handler.EditorWindow.Reuse;
    }

    if (Name is not "")
    {
      if (
        new List<string>(
          System.Enum.GetNames<Client.File.Handler.EditorProfile>()
        )
          .Find(
            pn => pn.StartsWith(
              Name,
              System.StringComparison.OrdinalIgnoreCase
            )
          ) is { } profileName
        )
      {
        profile = System.Enum.Parse<Client.File.Handler.EditorProfile>(
          profileName
        );
      }
      else
      {
        _ = arguments.AddFirst(Name);
      }
    }
  }

  sealed override private protected void Process() => Client.File.Handler.Edit(
    InCurrentLocation
      && Path is ""
      && (
        Pwd() == Client.Environment.Folder.Code()
        || Pwd() == Client.Environment.Folder.Home()
      )
        ? string.Empty
        : ReanchorPath(Path),
    profile,
    window,
    arguments
  );
}
