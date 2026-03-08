namespace Module.Commands.Shell.Start.Workspace;

abstract public class VirtualStartWorkspace() : CoreCommand(true)
{
  private enum VSCodeProfile
  {
    Default,
    Setting,
    Svelte,
    Python,
    C,
  }

  private const string FlagNewWindow = "--new-window";
  private const string FlagReuseWindow = "--reuse-window";

  abstract public string Path { set; }
  private protected string path = string.Empty;

  [Parameter(Position = 1)]
  [ValidateNotNullOrWhiteSpace]
  [Tab.EnumCompletions(typeof(VSCodeProfile))]
  public string Name
  {
    set
    {
      List<string> profileNames = [
        .. Enum.GetNames<VSCodeProfile>(),
      ];

      var profileName = profileNames.Find(
        pn => pn.StartsWith(
          value,
          System.StringComparison.OrdinalIgnoreCase
        )
      );

      if (profileName is not null)
      {
        profile = profileName switch
        {
          "Default" => string.Empty,
          "C" => "C++",
          var pn => pn,
        };

        return;
      }

      ArgumentList = [
        value,
        .. ArgumentList,
      ];
    }
  }
  private string profile = string.Empty;

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
    private get;
    set;
  }

  [Parameter]
  public SwitchParameter ReuseWindow
  {
    private get;
    set;
  }

  sealed override private protected void Preprocess()
  {
    if (Window || ReuseWindow)
    {
      _ = ArgumentList.RemoveAll(a => a is FlagNewWindow);
      _ = ArgumentList.RemoveAll(a => a is FlagReuseWindow);
    }
  }

  sealed override private protected void Process()
  {
    path = InCurrentLocation
      && path is ""
      && (
        Pwd() == Client.Environment.Known.Folder.Code()
        || Pwd() == Client.Environment.Known.Folder.Home()
      )
        ? string.Empty
        : ReanchorPath(path);

    if (name is "")
    {
      Client.File.Handler.Edit(
        path,
        newWindow,
        reuseWindow,
        ArgumentList
      );
    }
    else
    {
      Client.File.Handler.Edit(
        path,
        profile,
        newWindow,
        reuseWindow,
        ArgumentList
      );
    }
  }
}
