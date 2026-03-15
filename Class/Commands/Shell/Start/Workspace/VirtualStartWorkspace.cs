namespace PowerModule.Commands.Shell.Start.Workspace;

abstract public class VirtualStartWorkspace() : CoreCommand(true)
{
  sealed class EditorProfileCompletionsAttribute : Tab.CompletionsAttribute<HashSet<string>>
  {
    internal EditorProfileCompletionsAttribute() : base(
      Client.File.Editor.Profile
    ) => Casing = Tab.CompletionCase.Lower;
  }

  string profile = string.Empty;
  string? argument;

  abstract public string Path
  { private protected get; init; }

  [Parameter(Position = 1)]
  [ValidateNotNullOrWhiteSpace]
  [EditorProfileCompletions]
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
    private get => window is Client.File.Editor.Window.New;
    set => window = Client.File.Editor.Window.New;
  }
  Client.File.Editor.Window window;

  [Parameter]
  public SwitchParameter ReuseWindow
  { private get; init; }

  sealed override private protected void Preprocess()
  {
    if (ReuseWindow && !Window)
    {
      window = Client.File.Editor.Window.Reuse;
    }

    if (Name is not "")
    {
      if (
        new List<string>(
           Client.File.Editor.Profile
        ).Find(
          pn => pn.StartsWith(
            Name,
            System.StringComparison.OrdinalIgnoreCase
          )
        ) is { } profileName
      )
      {
        profile = profileName;
      }
      else
      {
        argument = Name;
      }
    }
  }

  sealed override private protected void Process() => Client.File.Editor.Edit(
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
