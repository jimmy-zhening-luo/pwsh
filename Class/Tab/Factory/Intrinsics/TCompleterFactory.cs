namespace PowerModule.Tab.Factory.Intrinsics;

[System.AttributeUsage(
  System.AttributeTargets.Property
  | System.AttributeTargets.Field
)]
abstract class TCompleterFactory(CompletionCase Case = default) : ArgumentCompleterAttribute, ICompleterFactory
{
  public CompletionCase Case
  { get; init; } = Case;

  abstract public Completers.Intrinsics.ICompleter Create();
}
