namespace PowerModule.Commands.Shell.Start.Workspace;

using static Client.File.Handler;

abstract public class VirtualStartWorkspace() : CoreCommand(true)
{
  EditorProfile profile;
  string? argument;

  abstract public string Path
  { private protected get; init; }

  [Parameter(Position = 1)]
  [ValidateNotNullOrWhiteSpace]
  [Tab.EnumCompletions(typeof(EditorProfile))]
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
  public string[] ArgumentList
  { private get; init; } = [];

  [Parameter]
  public SwitchParameter Window
  {
    private get => window is EditorWindow.New;
    set => window = EditorWindow.New;
  }
  EditorWindow window;

  [Parameter]
  public SwitchParameter ReuseWindow
  { private get; init; }

  sealed override private protected void Preprocess()
  {
    if (ReuseWindow && !Window)
    {
      window = EditorWindow.Reuse;
    }

    if (Name is not "")
    {
      if (
        new List<string>(
          System.Enum.GetNames<EditorProfile>()
        )
          .Find(
            pn => pn.StartsWith(
              Name,
              System.StringComparison.OrdinalIgnoreCase
            )
          ) is { } profileName
        )
      {
        profile = System.Enum.Parse<EditorProfile>(
          profileName
        );
      }
      else
      {
        argument = Name;
      }
    }
  }

  sealed override private protected void Process() => Edit(
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
    argument is null
      ? ArgumentList
      : [
          argument,
          .. ArgumentList,
        ]
  );
}
