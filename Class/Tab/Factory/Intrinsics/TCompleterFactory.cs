namespace PowerModule.Tab.Factory.Intrinsics;

[System.AttributeUsage(
  System.AttributeTargets.Property
  | System.AttributeTargets.Field
)]
abstract class TCompleterFactory : ArgumentCompleterAttribute, ICompleterFactory
{
  virtual public CompletionCase Case
  { get; init; }

  abstract public Completers.Intrinsics.ICompleter Create();
}
