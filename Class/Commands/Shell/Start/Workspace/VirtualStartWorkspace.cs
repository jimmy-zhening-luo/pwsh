namespace Module.Commands.Shell.Start.Workspace;

abstract public class VirtualStartWorkspace() : CoreCommand(true)
{
  private const string FlagNewWindow = "--new-window";
  private const string FlagReuseWindow = "--reuse-window";

  abstract public string Path { set; }
  private protected string path = string.Empty;

  [Parameter(Position = 1)]
  [ValidateNotNullOrWhiteSpace]
  [Tab.EnumCompletions(typeof(Client.File.Handler.EditorProfile))]
  public string Name
  {
    set
    {
      List<string> profileNames = [
        .. System.Enum.GetNames<Client.File.Handler.EditorProfile>(),
      ];

      var profileName = profileNames.Find(
        pn => pn.StartsWith(
          value,
          System.StringComparison.OrdinalIgnoreCase
        )
      );

      if (profileName is not null)
      {
        profile = System.Enum.Parse<Client.File.Handler.EditorProfile>(profileName);

        return;
      }

      ArgumentList = [
        value,
        .. ArgumentList,
      ];
    }
  }
  private Client.File.Handler.EditorProfile profile;

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
    private get;
    set;
  } = [];

  [Parameter]
  public SwitchParameter Window
  {
    private get => window is Client.File.Handler.EditorWindow.New;
    set => window = Client.File.Handler.EditorWindow.New;
  }
  private Client.File.Handler.EditorWindow window;

  [Parameter]
  public SwitchParameter ReuseWindow
  {
    private get;
    set;
  }

  sealed override private protected void Preprocess()
  {
    if (ReuseWindow && !Window)
    {
      window = Client.File.Handler.EditorWindow.Reuse;
    }
  }

  sealed override private protected void Process() => Client.File.Handler.Edit(
    InCurrentLocation
      && path is ""
      && (
        Pwd() == Client.Environment.Known.Folder.Code()
        || Pwd() == Client.Environment.Known.Folder.Home()
      )
        ? string.Empty
        : ReanchorPath(path),
    profile,
    window,
    ArgumentList
  );
}
