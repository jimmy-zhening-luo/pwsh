namespace Module.Commands.Pwsh.Model.Type;

[Cmdlet(
  VerbsCommon.Get,
  "Type"
)]
[Alias("ty", "typeof")]
[OutputType(typeof(System.Reflection.TypeInfo))]
public sealed class GetType : CoreCommand
{
  [Parameter(
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true,
    HelpMessage = "The object whose type to check"
  )]
  public required object InputObject { get; set; }

  private protected sealed override void Process()
  {
    if (InputObject is null)
    {
      Throw(
        new System.ArgumentException(
          "Cannot get type of null InputObject.",
          nameof(InputObject)
        ),
        ErrorCategory.InvalidArgument,
        InputObject
      );
    }

    if (InputObject is PSObject inputPsObject)
    {
      WriteObject(
        inputPsObject
          .BaseObject
          .GetType()
      );
    }
    else
    {
      WriteObject(InputObject.GetType());
    }
  }
}
