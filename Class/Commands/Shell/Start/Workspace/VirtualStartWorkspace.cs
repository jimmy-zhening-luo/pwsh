namespace PowerModule.Commands.Shell.Start.Workspace;

abstract public class VirtualStartWorkspace() : CoreCommand(true)
{
  private Client.File.Handler.EditorProfile profile;

  abstract public string Path
  { private protected get; set; }

  [Parameter(Position = 1)]
  [ValidateNotNullOrWhiteSpace]
  [Tab.EnumCompletions(typeof(Client.File.Handler.EditorProfile))]
  public string Name
  { private get; init; } = string.Empty;

  [Parameter(
    Position = 2,
    ValueFromRemainingArguments = true,
    DontShow = true,
    HelpMessage = "Additional arguments"
  )]
  [ValidateLength(1, int.MaxValue)]
  [Tab.PathCompletions]
  public Collection<string> ArgumentList
  { private get; init; } = [];

  [Parameter]
  public SwitchParameter Window
  {
    private get => window is Client.File.Handler.EditorWindow.New;
    set => window = Client.File.Handler.EditorWindow.New;
  }
  private Client.File.Handler.EditorWindow window;

  [Parameter]
  public SwitchParameter ReuseWindow
  { private get; set; }

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
        ArgumentList.Insert(default, Name);
      }
    }
  }

  sealed override private protected void Process() => Client.File.Handler.Edit(
    InCurrentLocation
      && Path is ""
      && (
        Pwd() == Client.Environment.Known.Folder.Code()
        || Pwd() == Client.Environment.Known.Folder.Home()
      )
        ? string.Empty
        : ReanchorPath(Path),
    profile,
    window,
    ArgumentList
  );
}
