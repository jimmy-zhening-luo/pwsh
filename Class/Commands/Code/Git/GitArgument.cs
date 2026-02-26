namespace Module.Commands.Code.Git;

public static partial class GitArgument
{
  [System.Text.RegularExpressions.GeneratedRegex(
    @"^(?>(?=.*[*=])(?>.+)|-(?>\w|(?>-\w[-\w]*\w)))$"
  )]
  public static partial System.Text.RegularExpressions.Regex Regex();

  [System.Text.RegularExpressions.GeneratedRegex(
    @"^(?=.)(?>HEAD)?(?<branching>(?>~|\^)?)(?<step>(?>\d{0,10}))$"
  )]
  public static partial System.Text.RegularExpressions.Regex TreeRegex();
}
